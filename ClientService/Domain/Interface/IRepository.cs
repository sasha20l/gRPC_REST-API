namespace Domain.Interface
{
    public interface IRepository<T> where T : class
    {
        IReadOnlyList<T> GetAll();
        T GetById(long id);
        long Create(T item);
        bool Update(T item);
        bool Delete(long id);
    }
}
