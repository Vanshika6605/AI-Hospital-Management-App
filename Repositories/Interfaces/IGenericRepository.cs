using System.Linq.Expressions;

namespace AIHospitalManagementSys.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = ""
        );
        
        Task<T?> GetByIdAsync(object id);
        
        Task<T?> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> filter,
            string includeProperties = ""
        );

        Task AddAsync(T entity);
        
        void Update(T entity);
        
        void Remove(T entity);
        
        Task SaveAsync();
    }
}
