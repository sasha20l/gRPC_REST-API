namespace Infrastructure.Interface
{
    public interface IRepository<T> where T : class
    {
        IReadOnlyList<T> GetAll();
        T GetById(long id);
        Task<long> Create(T item);
        Task<bool> Update(T item);
        Task<bool> Delete(long id);
    }
}
