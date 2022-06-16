using ServiceDesk.Data.Features.User;
using System.Collections.Generic;

namespace ServiceDesk.Data.Interfaces
{
    public interface IUserRepository//<T> where T : BaseEntity
    {
        bool Add(UserCommand model);

        bool Remove(int id);

        bool Update(UserCommand model);

        IEnumerable<UserResponse> FindById(int id);

        IEnumerable<UserResponse> FindByName(string userName);

        IEnumerable<UserResponse> FindUserByDepartmentId(int departmentId);

        IEnumerable<UserResponse> FindUserByDepartmentId(int departmentId,bool isAdmin);

        IEnumerable<UserResponse> FindAllUser();

        int FindCountExecuting(int userId);

        IEnumerable<UserResponse> FindExecutingList(int departmentId);

        IEnumerable<UserResponse> FindAllUserByTaskId(int taskId);

        IEnumerable<UserResponse> FindUserByTaskId(int taskId, int departmentId);

        IEnumerable<UserResponse> FindUserByIssueId(int issueId, int departmentId);

        IEnumerable<UserResponse> FindAll();

        //Task<IEnumerable<T>> FindByProcedureAsync();
        IEnumerable<UserResponse> FindByFunction();

        bool ExistUser(string userId);

        bool DepartmentView(string userId);

        string FindLeaderEmail(int departmentId);

        string FindLeaderEmail(string departmentList, int departmentId);

        string FindPersonnelEmail(int departmentId);

        string FindPersonnelEmail(string userList, int departmentId);
    }
}