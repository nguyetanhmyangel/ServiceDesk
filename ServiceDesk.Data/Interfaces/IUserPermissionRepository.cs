using ServiceDesk.Data.Features.UserPermission;
using System.Collections.Generic;

namespace ServiceDesk.Data.Interfaces
{
    public interface IUserPermissionRepository//<T> where T : BaseEntity
    {
        bool Add(UserPermissionCommand model);

        bool Remove(int id);

        bool ChangePermission(UserPermissionCommand model);

        bool Update(UserPermissionCommand model);

        IEnumerable<UserPermissionResponse> FindById(int id);

        IEnumerable<UserPermissionResponse> FindByRoleId(int id);

        IEnumerable<UserPermissionResponse> FindAll();

        //Task<IEnumerable<T>> FindByProcedureAsync();
        IEnumerable<UserPermissionResponse> FindByFunction();
    }
}