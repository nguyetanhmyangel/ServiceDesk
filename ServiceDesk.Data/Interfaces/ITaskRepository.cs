using System;
using System.Collections.Generic;
using ServiceDesk.Data.Features.Task;
using ServiceDesk.Data.ViewModels.TimeLife;

namespace ServiceDesk.Data.Interfaces
{
    public interface ITaskRepository //where T : BaseEntity
    {
        bool Add(TaskCommand model);

        bool Remove(int id);

        void Update(string multiDepartmentIds, int issueId);

        void Update(string multiDepartmentIds, string multiUserIds, int issueId);

        bool Update(TaskCommand model, IEnumerable<object> multiDepartments);

        bool Update(TaskCommand model, IEnumerable<object> multiDepartments, IEnumerable<object> multiUsers);

        IEnumerable<TaskResponse> FindById(int id);

        IEnumerable<TaskTimeLife> FindForTimLife(int issueId);

        IEnumerable<TaskResponse> FindByDepartmentId(int departmentId, int statusId, DateTime fromDate, DateTime toDate);

        IEnumerable<TaskResponse> FindAll(int statusId, DateTime fromDate, DateTime toDate);

        IEnumerable<TaskResponse> FindAllByFunction();

    }
}