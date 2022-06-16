using ServiceDesk.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceDesk.Data.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task Add(T item);

        Task Remove(int id);

        Task Update(T item);

        Task<T> FindById(int id);

        Task<IEnumerable<T>> FindAll();

        Task<IEnumerable<T>> FindSingle();

        Task<IEnumerable<T>> FindByProcedure();

        Task<IEnumerable<T>> FindByFunction();
    }
}