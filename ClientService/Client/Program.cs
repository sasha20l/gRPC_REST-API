
using GameServiceNamespace;
using Grpc.Net.Client;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using static GameServiceNamespace.GameServiceProto;

namespace Client
{
    
    class Program
    {
        static async Task Main(string[] args)
        {
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            using var grpcChannel = GrpcChannel.ForAddress("https://localhost:5001");

            GameServiceProtoClient gameServiceProtoClient = new GameServiceProtoClient(grpcChannel);

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nВведите команду:");
                Console.WriteLine("1 - Просмотр баланса");
                Console.WriteLine("2 - Получение списка игр");
                Console.WriteLine("3 - Подключение к игре");
                Console.WriteLine("4 - Создать нового пользователя");
                Console.WriteLine("5 - Перевести деньги");
                Console.WriteLine("6 - Создать матч");
                Console.WriteLine("0 - Выход");
                Console.Write("Команда: ");
                var command = Console.ReadLine();

                switch (command)
                {
                    case "1":

                        Console.Write("Введите идентификатор пользователя: ");
                        var userIdInput = Console.ReadLine();
                        if (long.TryParse(userIdInput, out long userId))
                        {
                            try
                            {
                                var plId = new PlayerId
                                {
                                    Id = Int32.Parse(userIdInput),
                                };

                                var response = await gameServiceProtoClient.GetBalanceAsync(plId);
                                if (response != null)
                                {
                                    Console.WriteLine($"Баланс пользователя {userId}: {response?.Balance}");
                                }
                                else
                                {
                                    Console.WriteLine($"Ошибка получения баланса");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ошибка при вызове REST API: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Неверный идентификатор пользователя");
                        }
                        break;

                    case "2":

                        try
                        {
                            var emptyRequest = new EmptyRequest();
                            var gameList = await gameServiceProtoClient.GetGamesAsync(emptyRequest);
                            Console.WriteLine("Список игр, ожидающих второго игрока:");
                            foreach (var game in gameList.Games)
                            {
                                Console.WriteLine($"Матч ID: {game.Id}, Ставка: {game.Stake}, Статус: {game.Status}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при получении списка игр: {ex.Message}");
                        }
                        break;

                    case "3":
                        // Подключение к игре через gRPC и выбор в игре "Камень-Ножницы-Бумага"
                        Console.Write("Введите ID матча: ");
                        var matchId = Console.ReadLine();
                        Console.Write("Введите ваш идентификатор игрока: ");
                        var playerId = Console.ReadLine();
                        Console.Write("Выберите: К (Камень), N (Ножницы) или B (Бумага): ");
                        var playerChoice = Console.ReadLine();

                        var joinRequest = new JoinGameRequest
                        {
                            MatchId = matchId,
                            PlayerId = playerId,
                            PlayerChoice = playerChoice
                        };

                        try
                        {
                            var joinResponse = await gameServiceProtoClient.JoinGameAsync(joinRequest);
                            Console.WriteLine($"Результат игры: {joinResponse.Result}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при подключении к игре: {ex.Message}");
                        }
                        break;
                    case "4":
                        
                        Console.Write("Введите имя нового пользователя: ");
                        var name = Console.ReadLine();
                        Console.Write("Введите баланс: ");
                        var balance = Console.ReadLine();
                        
           
                        var user = new UserRequest
                        {
                            UserName = name,
                            Balance = double.Parse(balance, CultureInfo.InvariantCulture)
                        };

                        try
                        {
                            var joinResponse = gameServiceProtoClient.CreateUser(user);
                            Console.Write(joinResponse.Message);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при создании пользователя");
                        }
                        
                        break;
                    case "5":
                        {
                            // Реализуем логику TransferMoney 
                            Console.Write("Введите ID отправителя (FromUser): ");
                            var fromUserStr = Console.ReadLine();

                            Console.Write("Введите ID получателя (ToUser): ");
                            var toUserStr = Console.ReadLine();

                            Console.Write("Введите сумму перевода: ");
                            var amountStr = Console.ReadLine();

                            if (!long.TryParse(fromUserStr, out long fromUserId))
                            {
                                Console.WriteLine("Неверный идентификатор отправителя");
                                break;
                            }
                            if (!long.TryParse(toUserStr, out long toUserId))
                            {
                                Console.WriteLine("Неверный идентификатор получателя");
                                break;
                            }
                            if (!double.TryParse(amountStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double amount))
                            {
                                Console.WriteLine("Неверная сумма");
                                break;
                            }

                            var transferRequest = new TransferRequest
                            {
                                FromUserId = fromUserId,
                                ToUserId = toUserId,
                                Amount = amount
                            };

                            try
                            {
                                // TransferMoney возвращает RpcStatus с сообщением об успехе/ошибке
                                var response = await gameServiceProtoClient.TransferMoneyAsync(transferRequest);
                                Console.WriteLine($"Ответ сервера: {response.Message}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Ошибка при переводе денег: " + ex.Message);
                            }
                            break;
                        }
                    case "6":
                        {
                            // Создать матч (CreateMatch)
                            Console.Write("Введите ID игрока, создающего матч: ");
                            var player1Str = Console.ReadLine();

                            Console.Write("Введите сумму ставки: ");
                            var stakeStr = Console.ReadLine();

                            if (!long.TryParse(player1Str, out long player1Id))
                            {
                                Console.WriteLine("Неверный идентификатор игрока");
                                break;
                            }
                            if (!double.TryParse(stakeStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double stake))
                            {
                                Console.WriteLine("Неверная сумма ставки");
                                break;
                            }

                            var createMatchRequest = new CreateMatchRequest
                            {
                                Player1Id = player1Id,
                                Stake = stake
                            };

                            try
                            {
                                // Метод CreateMatchAsync возвращает RpcStatus (с сообщением)
                                var response = await gameServiceProtoClient.CreateMatchAsync(createMatchRequest);
                                Console.WriteLine($"Ответ сервера: {response.Message}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Ошибка при создании матча: " + ex.Message);
                            }
                            break;
                        }

                    case "0":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Неверная команда");
                        break;
                }
            }
          
        }
    }
    }
