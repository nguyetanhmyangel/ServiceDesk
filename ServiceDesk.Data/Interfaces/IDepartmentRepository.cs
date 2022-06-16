using ServiceDesk.Data.Features.Department;
using System.Collections.Generic;

namespace ServiceDesk.Data.Interfaces
{
    public interface IDepartmentRepository
    {
        IEnumerable<DepartmentResponse> FindById(int id);

        IEnumerable<DepartmentResponse> FindByDivisionId(int divisionId);

        IEnumerable<DepartmentResponse> FindByDivisionId(int divisionId, int currentDepartmentId);

        IEnumerable<DepartmentResponse> FindByDivisionId();

        IEnumerable<DepartmentResponse> FindByUser(int roleId, int departmentId);

        IEnumerable<DepartmentResponse> FindByIssueId(int issueId);

        IEnumerable<DepartmentResponse> FindAll();

        IEnumerable<DepartmentResponse> FindByTaskId(int taskId);

        IEnumerable<DepartmentResponse> FindAllByFunction();
    }
}