using Domain.Interface;
using Infrastructure.DbCont;

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

        public long Create(User item)
        {
            _context.User.Add(item);
            _context.SaveChanges();
            return item.UserId;
        }

        public bool Delete(long id)
        {
                User entity = _context.User.Find(id);
                entity.IsDeleted = true;
                return Commit();
        }

        public IReadOnlyList<User> GetAll()
        {
                return _context.User.Where(x => x.IsDeleted == false).ToList();
        }

        public User GetById(long id)
        {
                return _context.User.FirstOrDefault(x => x.IsDeleted == false && x.UserId == id);
        }

        public bool Update(User item)
        {
                if (item == null)
                    throw new Exception("Cargo is null.");
                User employee = GetById(item.UserId);

                employee.UserId = item.UserId;
                employee.UserName = item.UserName;
                employee.IsDeleted = item.IsDeleted;
                employee.Balance = item.Balance;
                employee.CreatedAt = item.CreatedAt;


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


