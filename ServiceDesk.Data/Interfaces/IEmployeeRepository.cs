using ServiceDesk.Data.Features.Employee;
using System.Collections.Generic;

namespace ServiceDesk.Data.Interfaces
{
    public interface IEmployeeRepository //where T : BaseEntity
    {
        //bool Add(EmployeeViewModel model);

        //bool Remove(int id);

        //bool Update(EmployeeViewModel model);

        IEnumerable<EmployeeResponse> FindById(int id);

        IEnumerable<EmployeeResponse> FindAll();

        IEnumerable<EmployeeResponse> FindInformation(string employeeId);

        int FindByEmployeeId(string employeeId);

        int ExistEmployeeId(string employeeId);

        IEnumerable<EmployeeResponse> FindByEmployeeExecuting(int departmentId, int roleId);

        IEnumerable<ExecutingResponse> FindByDetail(int userId);

        //IEnumerable<EmployeeViewModel> FindAllByFunction();
    }
}