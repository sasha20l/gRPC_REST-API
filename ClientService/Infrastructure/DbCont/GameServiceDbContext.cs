using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbCont
{
    public class GameServiceDbContext : DbContext
    {
        public GameServiceDbContext(DbContextOptions<GameServiceDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<MatchHistory> MatchHistory { get; set; }
        public DbSet<GameTransactions> GameTransactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Индексы для быстрого поиска
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName);

            modelBuilder.Entity<MatchHistory>()
                .HasIndex(m => m.fkPlayer1Id);
            modelBuilder.Entity<MatchHistory>()
                .HasIndex(m => m.fkPlayer2Id);

            modelBuilder.Entity<GameTransactions>()
                .HasIndex(gt => gt.fkFromUserId);
            modelBuilder.Entity<GameTransactions>()
                .HasIndex(gt => gt.fkToUserId);
        }
    }
}
