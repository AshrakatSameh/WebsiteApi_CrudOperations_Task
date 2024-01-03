using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IGenericRepo<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);

        //specification pattern
        Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T,
            object>>[] includes);

        Task<T> GetByIdAsync2(int id, params Expression<Func<T, object>>[] includes);
    }
}
