
using Infrastructure.DbCont;
using Infrastructure.Interface;

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

        public async Task<long> Create(MatchHistory item)
        {
            await _context.MatchHistory.AddAsync(item);
            await _context.SaveChangesAsync();
            return item.MatchHistoryId;
        }

        public async Task<bool> Delete(long id)
        {
                MatchHistory entity = await _context.MatchHistory.FindAsync(id);
                entity.IsDeleted = true;
                return await Commit();
        }

        public IReadOnlyList<MatchHistory> GetAll()
        {
                return _context.MatchHistory.Where(x => x.IsDeleted == false).ToList();
        }

        public MatchHistory GetById(long id)
        {
                return _context.MatchHistory.FirstOrDefault(x => x.IsDeleted == false && x.MatchHistoryId == id);
        }

        public async Task<bool> Update(MatchHistory item)
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


