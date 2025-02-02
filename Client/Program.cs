using Domain.Responce;
using GameServiceNamespace;
using Grpc.Net.Client;
using System.Net.Http.Json;

namespace Client
{
    
    class Program
    {
        static async Task Main(string[] args)
        {
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
      
            using var grpcChannel = GrpcChannel.ForAddress("https://localhost:5001");

          
            var grpcClient = new GameServiceProto.GameServiceProtoClient(grpcChannel);

            using var httpClient = new HttpClient(httpHandler) { BaseAddress = new Uri("https://localhost:5002") };

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nВведите команду:");
                Console.WriteLine("1 - Просмотр баланса");
                Console.WriteLine("2 - Получение списка игр");
                Console.WriteLine("3 - Подключение к игре");
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
                          
                                var response = await httpClient.GetAsync($"/api/users/{userId}/balance");
                                if (response.IsSuccessStatusCode)
                                {
                                    var balanceResponse = await response.Content.ReadFromJsonAsync<BalanceResponse>();
                                    Console.WriteLine($"Баланс пользователя {userId}: {balanceResponse?.Balance}");
                                }
                                else
                                {
                                    Console.WriteLine($"Ошибка получения баланса: {response.StatusCode}");
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
                            var gameList = await grpcClient.GetGamesAsync(emptyRequest);
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
                            var joinResponse = await grpcClient.JoinGameAsync(joinRequest);
                            Console.WriteLine($"Результат игры: {joinResponse.Result}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ошибка при подключении к игре: {ex.Message}");
                        }
                        break;

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
