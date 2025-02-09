using GameServiceNamespace;
using Grpc.Core;
using Infrastructure.DbCont;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GameServer
{
    public class GameService : GameServiceProto.GameServiceProtoBase
    {
        private readonly GameServiceDbContext _dbContext;
        private static readonly Dictionary<string, string> _playerChoices = new Dictionary<string, string>();

        public GameService(GameServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<RpcStatus> TransferMoney(TransferRequest request, ServerCallContext context)
        {
            RpcStatus rpcStatus = new RpcStatus();

            var fromUser = await _dbContext.User.FirstOrDefaultAsync(u => u.UserId == request.FromUserId && !u.IsDeleted);
            var toUser = await _dbContext.User.FirstOrDefaultAsync(u => u.UserId == request.ToUserId && !u.IsDeleted);

            if (fromUser == null || toUser == null)
            {
                rpcStatus.Message = "Пользователь не найден";
                return rpcStatus;
            }

            if (fromUser.Balance < request.Amount)
            {
                rpcStatus.Message = "Недостаточно средств";
                return rpcStatus;
            }

            fromUser.Balance -= request.Amount;
            toUser.Balance += request.Amount;

            var transaction = new GameTransactions
            {
                fkFromUserId = fromUser.UserId,
                fkToUserId = toUser.UserId,
                Amount = request.Amount,
                Reason = "Manual transfer",
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            _dbContext.GameTransactions.Add(transaction);
            await _dbContext.SaveChangesAsync();

            rpcStatus.Message = "Перевод выполнен успешно";
            return rpcStatus;
        }

        public override async Task<RpcStatus> CreateMatch(CreateMatchRequest request, ServerCallContext context)
        {
            RpcStatus rpcStatus = new RpcStatus();

            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.UserId == request.Player1Id && !u.IsDeleted);
            if (user == null)
            {
                rpcStatus.Message = "Пользователь не найден";
                return rpcStatus;
            }

            if (user.Balance < request.Stake)
            {
                rpcStatus.Message = "Недостаточно средств для создания матча";
                return rpcStatus;
            }

            var match = new MatchHistory
            {
                fkPlayer1Id = request.Player1Id,
                Stake = request.Stake,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            _dbContext.MatchHistory.Add(match);
            await _dbContext.SaveChangesAsync();

            rpcStatus.Message = $"ID матча = {match.MatchHistoryId}";

            return rpcStatus;
        }

        // Получение списка игр, в которых пока нет второго игрока
        public override async Task<GameList> GetGames(EmptyRequest request, ServerCallContext context)
        {
            var games = await _dbContext.MatchHistory
                .Where(m => m.fkPlayer2Id == null)
                .Select(m => new Game
                {
                    Id = m.MatchHistoryId.ToString(),
                    Stake = (double)m.Stake,
                    Status = "Waiting for player"
                })
                .ToListAsync();

            var response = new GameList();
            response.Games.AddRange(games);
            return response;
        }

        // Обработка подключения к игре и выбора "Камень-Ножницы-Бумага"
        public override async Task<GameResult> JoinGame(JoinGameRequest request, ServerCallContext context)
        {
            if (!long.TryParse(request.MatchId, out long matchId))
            {
                return new GameResult { Result = "Неверный идентификатор матча" };
            }

            // Находим матч
            var match = await _dbContext.MatchHistory.FirstOrDefaultAsync(m => m.MatchHistoryId == matchId && !m.IsDeleted);
            if (match == null)
            {
                return new GameResult { Result = "Матч не найден" };
            }

            // Преобразуем идентификатор игрока
            if (!long.TryParse(request.PlayerId, out long playerId))
            {
                return new GameResult { Result = "Неверный идентификатор игрока" };
            }

            // Если игрок еще не добавлен в матч, добавляем его как второго игрока
            if (match.fkPlayer2Id == null && match.fkPlayer1Id != playerId)
            {
                match.fkPlayer2Id = playerId;
                await _dbContext.SaveChangesAsync();
            }

            // Сохраняем выбор игрока (упрощенно: ключ – составной из matchId и playerId)
            var key = GetKey(matchId, playerId);
            _playerChoices[(key)] = request.PlayerChoice;

            // Если оба игрока уже сделали выбор, определяем победителя
            if (BothPlayersMadeChoice(match, matchId))
            {
                var result = CalculateResult(match, matchId);
                // Сохраняем результат матча
                match.fkWinnerId = result.winnerId;
                await _dbContext.SaveChangesAsync();

                // Создаем транзакции: перевод средств от проигравшего к победителю (если не ничья)
                if (result.winnerId != null)
                {
                    long loserId = (result.winnerId == match.fkPlayer1Id) ? match.fkPlayer2Id.Value : match.fkPlayer1Id;
                    var transaction = new GameTransactions
                    {
                        fkFromUserId = loserId,
                        fkToUserId = result.winnerId.Value,
                        Amount = match.Stake,
                        Reason = "Win",
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    };
                    _dbContext.GameTransactions.Add(transaction);
                    await _dbContext.SaveChangesAsync();
                }

                // Очистка состояния
                RemoveChoices(match, matchId);
                return new GameResult { Result = result.message };
            }
            else
            {
                return new GameResult { Result = "Ожидание второго игрока" };
            }
        }

        // Вспомогательные методы

        // Формируем ключ для хранения выбора игрока
        private string GetKey(long matchId, long playerId) => $"{matchId}_{playerId}";

        // Проверяем, сделали ли оба игрока выбор
        private bool BothPlayersMadeChoice(MatchHistory match, long matchId)
        {
            if (match.fkPlayer1Id == 0 || match.fkPlayer2Id == null)
                return false;

            var key1 = GetKey(matchId, match.fkPlayer1Id);
            var key2 = GetKey(matchId, match.fkPlayer2Id.Value);
            return _playerChoices.ContainsKey(key1) && _playerChoices.ContainsKey(key2);
        }

        // Определяем победителя по правилам игры "Камень-Ножницы-Бумага"
        private (long? winnerId, string message) CalculateResult(MatchHistory match, long matchId)
        {
            var key1 = GetKey(matchId, match.fkPlayer1Id);
            var key2 = GetKey(matchId, match.fkPlayer2Id.Value);

            var choice1 = _playerChoices[key1].ToUpper();
            var choice2 = _playerChoices[key2].ToUpper();

            // Если выборы равны – ничья
            if (choice1 == choice2)
                return (null, "Ничья");

            // Правила: Камень (К) побеждает Ножницы (N), Ножницы (N) побеждают Бумагу (B), Бумага (B) побеждает Камень (К)
            bool player1Wins = (choice1 == "К" && choice2 == "N")
                               || (choice1 == "N" && choice2 == "B")
                               || (choice1 == "B" && choice2 == "К");

            if (player1Wins)
                return (match.fkPlayer1Id, "Игрок 1 победил");
            else
                return (match.fkPlayer2Id, "Игрок 2 победил");
        }

        // Удаляем сохранённые выборы по завершении матча
        private void RemoveChoices(MatchHistory match, long matchId)
        {
            var key1 = GetKey(matchId, match.fkPlayer1Id);
            var key2 = GetKey(matchId, match.fkPlayer2Id.Value);
            _playerChoices.Remove(key1);
            _playerChoices.Remove(key2);
        }
    }

}
