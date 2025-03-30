namespace BackEnd.Service
{
    public interface ICollectionService<T>
    {
        Task<bool> Create(T enity);
        Task<bool> Delete(string id);
        Task<bool> Update(string id,T entity);
        Task<T> GetById(string id);
        Task<List<T>> GetAll();
    }
}
