using Infrastructure.DbCont;
using Infrastructure.Repositiry;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace TestTask1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(IPAddress.Any, 5002, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
            });

            builder.Services.AddGrpc();

            builder.Services.AddControllers();

            #region Конфигурация EF DbContext
            builder.Services.AddDbContext<GameServiceDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration["DatabaseOptions:ConnectionString"]);
            });
            #endregion

            #region Регистрация репозиториев
            RegisterRepositories(builder.Services);
            #endregion

            var app = builder.Build();

            app.UseRouting();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IGameTransactionsRepository, GameTransactionsRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMatchHistoryRepository, MatchHistoryRepository>();
        }
    }
}
