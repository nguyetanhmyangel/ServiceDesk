using ServiceDesk.Data.Features.RolePermission;
using System.Collections.Generic;

namespace ServiceDesk.Data.Interfaces
{
    public interface IRolePermissionRepository//<T> where T : BaseEntity
    {
        bool Add(RolePermissionCommand model);

        bool Remove(int id);

        bool ChangePermission(RolePermissionCommand model);

        bool Update(RolePermissionCommand model);

        IEnumerable<RolePermissionResponse> FindById(int id);

        IEnumerable<RolePermissionResponse> FindByRoleId(int id);

        IEnumerable<RolePermissionResponse> FindAll();

        //Task<IEnumerable<T>> FindByProcedureAsync();
        IEnumerable<RolePermissionResponse> FindByFunction();
    }
}