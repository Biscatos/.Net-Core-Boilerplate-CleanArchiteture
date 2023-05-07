using System.Linq.Expressions;

namespace Core.Application.Interfaces.System
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync(long id);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> equater);
        Task<T> GetByGUIDAsync(Guid id);
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize);
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entitys);
        Task UpdateAsync(T entity);
        Task UpdateAsync(Action<T> setter, Func<T, bool> equater);
        Task DeleteAsync(T entity);
        Task DeleteAsync(Func<T, bool> equater);
    }

}
