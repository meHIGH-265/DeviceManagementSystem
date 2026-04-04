using DeviceManagementSystem.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceManagementSystem.Repository
{
    public interface IRepository<E, T>
    {
        Task<IEnumerable<E>> GetAllAsync();

        Task<E?> GetByIdAsync(T id);

        Task<T> CreateAsync(E entity);

        Task<bool> UpdateAsync(E entity);

        Task<bool> DeleteAsync(T id);
    }
}
