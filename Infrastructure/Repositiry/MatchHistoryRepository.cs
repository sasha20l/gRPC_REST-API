using Domain.Interface;
using Infrastructure.DbCont;

namespace Infrastructure.Repositiry
{
    public interface IMatchHistoryRepository : IRepository<MatchHistory>
    {
    }
    public sealed class MatchHistoryRepository : IMatchHistoryRepository
    {
        private readonly GameServiceDbContext _context;
        public MatchHistoryRepository(GameServiceDbContext context)
        {
            _context = context;
        }

        public long Create(MatchHistory item)
        {
            _context.MatchHistory.Add(item);
            _context.SaveChanges();
            return item.MatchHistoryId;
        }

        public bool Delete(long id)
        {
                MatchHistory entity = _context.MatchHistory.Find(id);
                entity.IsDeleted = true;
                return Commit();
        }

        public IReadOnlyList<MatchHistory> GetAll()
        {
                return _context.MatchHistory.Where(x => x.IsDeleted == false).ToList();
        }

        public MatchHistory GetById(long id)
        {
                return _context.MatchHistory.FirstOrDefault(x => x.IsDeleted == false && x.MatchHistoryId == id);
        }

        public bool Update(MatchHistory item)
        {
                if (item == null)
                    throw new Exception("Cargo is null.");
                MatchHistory employee = GetById(item.MatchHistoryId);

                employee.MatchHistoryId = item.MatchHistoryId;
                employee.fkPlayer1Id = item.fkPlayer1Id;
                employee.IsDeleted = item.IsDeleted;
                employee.fkPlayer2Id = item.fkPlayer2Id;
                employee.Stake = item.Stake;
                employee.fkWinnerId = item.fkWinnerId;
                employee.CreatedAt = item.CreatedAt;
                employee.Player1 = item.Player1;
                employee.Player2 = item.Player2;
                employee.Winner = item.Winner;

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


