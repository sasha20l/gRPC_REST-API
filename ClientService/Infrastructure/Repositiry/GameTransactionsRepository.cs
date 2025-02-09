
using Infrastructure.DbCont;
using Infrastructure.Interface;

namespace Infrastructure.Repositiry
{
    public interface IGameTransactionsRepository : IRepository<GameTransactions>
    {
    }
    public sealed class GameTransactionsRepository : IGameTransactionsRepository
    {
        private readonly GameServiceDbContext _context;
        public GameTransactionsRepository(GameServiceDbContext context)
        {
            _context = context;
        }

        public async Task<long> Create(GameTransactions item)
        {
            await _context.GameTransactions.AddAsync(item);
            await _context.SaveChangesAsync();
            return item.GameTransactionsId;
        }

        public async Task<bool> Delete(long id)
        {
            GameTransactions entity = await _context.GameTransactions.FindAsync(id);
            entity.IsDeleted = true;
            return await Commit();
        }

        public IReadOnlyList<GameTransactions> GetAll()
        {
                return _context.GameTransactions.Where(x => x.IsDeleted == false).ToList();
        }

        public GameTransactions GetById(long id)
        {
                return _context.GameTransactions.FirstOrDefault(x => x.IsDeleted == false && x.GameTransactionsId == id);
        }

        public async Task<bool> Update(GameTransactions item)
        {
                if (item == null)
                    throw new Exception("GameTransactions is null.");
                GameTransactions employee = GetById(item.GameTransactionsId);

                employee.GameTransactionsId = item.GameTransactionsId;
                employee.fkFromUserId = item.fkFromUserId;
                employee.IsDeleted = item.IsDeleted;
                employee.fkToUserId = item.fkToUserId;
                employee.Amount = item.Amount;
                employee.Reason = item.Reason;
                employee.CreatedAt = item.CreatedAt;
                employee.FromUser = item.FromUser;
                employee.ToUser = item.ToUser;
                employee.IsDeleted = item.IsDeleted;

                await _context.SaveChangesAsync(true);
                return true;
        }

        private async Task<bool> Commit()
        {
            int count = await _context.SaveChangesAsync();
            return count > 0;
        }
    }
}


