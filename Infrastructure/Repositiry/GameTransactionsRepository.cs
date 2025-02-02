using Domain.Interface;
using Infrastructure.DbCont;

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

        public long Create(GameTransactions item)
        {
            _context.GameTransactions.Add(item);
            _context.SaveChanges();
            return item.GameTransactionsId;
        }

        public bool Delete(long id)
        {
                GameTransactions entity = _context.GameTransactions.Find(id);
                entity.IsDeleted = true;
                return Commit();
        }

        public IReadOnlyList<GameTransactions> GetAll()
        {
                return _context.GameTransactions.Where(x => x.IsDeleted == false).ToList();
        }

        public GameTransactions GetById(long id)
        {
                return _context.GameTransactions.FirstOrDefault(x => x.IsDeleted == false && x.GameTransactionsId == id);
        }

        public bool Update(GameTransactions item)
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

        _context.SaveChanges(true);
                return true;
        }

        private bool Commit()
        {
            int count = _context.SaveChanges();
            return count > 0;
        }
    }
}


