using ServiceDesk.Data.Features.Task;
using ServiceDesk.Data.Features.TaskExecuted;
using System;
using System.Collections.Generic;

namespace ServiceDesk.Data.Interfaces
{
    public interface ITaskExecuteRepository //where T : BaseEntity
    {
        bool Add(TaskExecuteCommand model);

        bool Remove(int id);

        bool Update(TaskExecuteCommand model);

        IEnumerable<TaskExecuteResponse> FindById(int id);

        IEnumerable<TaskExecuteResponse> FindByTaskId(int taskId);

        IEnumerable<TaskExecuteResponse> FindByUserId(string userName, int statusId, string languageId, DateTime fromDate, DateTime toDate);

        IEnumerable<TaskExecuteResponse> FindAll();

        IEnumerable<TaskExecuteResponse> FindAllByFunction();

        IEnumerable<TaskExecuteTimeLife> FindForTimLife(int taskId);

        IEnumerable<TaskExecuteResponse> FindByIssueId(int issueId, string languageId);

        IEnumerable<TaskExecuteTimeLife> FindForTimLife(IEnumerable<int> listTaskId);
    }
}