using ServiceDesk.Data.Features.Role;
using System.Collections.Generic;

namespace ServiceDesk.Data.Interfaces
{
    public interface IRoleRepository//<T> where T : BaseEntity
    {
        bool Add(RoleCommand model);

        bool Remove(int id);

        bool Update(RoleCommand model);

        IEnumerable<RoleResponse> FindById(int id);

        IEnumerable<RoleResponse> FindAll();

        //Task<IEnumerable<T>> FindByProcedureAsync();
        IEnumerable<RoleResponse> FindByFunction();

        IEnumerable<RoleResponse> FindByName(string roleName);
    }
}