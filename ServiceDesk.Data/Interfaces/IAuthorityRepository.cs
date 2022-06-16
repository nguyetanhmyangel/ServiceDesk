using Dapper;
using ServiceDesk.Data.Features.Employee;
using ServiceDesk.Data.Features.Login;
using System.Collections.Generic;
using ServiceDesk.Data.Features.User;

namespace ServiceDesk.Data.Interfaces
{
    public interface IAuthorityRepository//<T> where T : BaseEntity
    {
        int CurrentPrivilege { get; set; }
        string CurrentUserName { get; set; }
        string CurrentUserId { get; set; }

        //string CurrentLanguageId { get; set; }
        int CurrentDepartmentId { get; set; }

        int CurrentDivisionId { get; set; }
        int CurrentRoleId { get; set; }
        bool CanAdd { get; set; }
        bool CanRemove { get; set; }
        bool CanUpdate { get; set; }
        bool CanView { get; set; }
        bool CanSaveChange { get; }

        bool LoggedIn();

        bool IsAdmin();

        IEnumerable<EmployeeResponse> EmployeList(string userName);

        IEnumerable<UserResponse> FindByUserName(DynamicParameters parameters);

        IEnumerable<EmployeeResponse> HrFindByUserName(DynamicParameters parameters);

        bool PasswordSignIn(LoginResponse model);

        bool HrPasswordSignIn(LoginResponse model);

        bool UserAuthority(LoginResponse model);

        int GetRole(string userName);

        bool AutoLogin();

        void LoadPrivilege();

        bool ChangePassword(LoginResponse model);
    }
}