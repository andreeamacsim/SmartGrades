namespace BackEnd.Service
{
    public interface ICollectionService<T>
    {
        Task<bool> Create(T enity);
        Task<bool> Delete(T enity);
        Task<bool> Update(string id,T enity);
        Task<T> GetById(string id);
        Task<List<T>> GetAll();
    }
}
