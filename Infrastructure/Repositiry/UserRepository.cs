
using Infrastructure.DbCont;
using Infrastructure.Interface;

namespace Infrastructure.Repositiry
{
    public interface IUserRepository : IRepository<User>
    {
    }
    public sealed class UserRepository : IUserRepository
    {
        private readonly GameServiceDbContext _context;
        public UserRepository(GameServiceDbContext context)
        {
            _context = context;
        }

        public async Task<long> Create(User item)
        {
            try
            {    
                await _context.User.AddAsync(item);
                await _context.SaveChangesAsync();
                return item.UserId;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при сохранении: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Внутреннее исключение: {ex.InnerException.Message}");
                }
                return 0;
            }

        }

        public async Task<bool> Delete(long id)
        {
                User entity = await _context.User.FindAsync(id);
                entity.IsDeleted = true;
                return await Commit();
        }

        public IReadOnlyList<User> GetAll()
        {
                return _context.User.Where(x => x.IsDeleted == false).ToList();
        }

        public User GetById(long id)
        {
                return _context.User.FirstOrDefault(x => x.IsDeleted == false && x.UserId == id);
        }

        public async Task<bool> Update(User item)
        {
                if (item == null)
                    throw new Exception("Cargo is null.");
                User employee = GetById(item.UserId);

                employee.UserId = item.UserId;
                employee.UserName = item.UserName;
                employee.IsDeleted = item.IsDeleted;
                employee.Balance = item.Balance;
                employee.CreatedAt = item.CreatedAt;


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


