using ServiceDesk.Data.Features.Issue;
using System;
using System.Collections.Generic;

namespace ServiceDesk.Data.Interfaces
{
    public interface IIssuesRepository //where T : BaseEntity
    {
        bool Add(IssuesCommand model);

        bool Remove(int id);

        int CanCancelIssue(int issueId);

        bool CancelIssue(int id,string reason);

        bool Update(IssuesCommand model);

        bool Update(int id, int ratingValue);

        bool Update(string userId, int issueId, IEnumerable<object> multiValue);

        IEnumerable<IssuesResponse> FindById(int id);

        IEnumerable<IssuesResponse> FindByOwner(string employeeId, string languageId,  DateTime fromDate, DateTime toDate, int tagId, int statusId, int roleId, int departmentId);

        IEnumerable<IssuesResponse> FindByOwner(string employeeId, string languageId, DateTime fromDate, DateTime toDate);

        IEnumerable<IssuesResponse> FindByEmployeeId(string employeeId, string languageId, DateTime fromDate, DateTime toDate);

        IEnumerable<IssuesResponse> FindAll();

        IEnumerable<IssuesResponse> FindByDivision(int divisionId, int statusId, string languageId, DateTime fromDate, DateTime toDate);

        int ProcessedStatus(int issueId);

        IEnumerable<IssueTimeLifeResponse> FindForTimeLife(int issueId, string languageId, string userId);

        IEnumerable<IssuesResponse> FindAllByFunction();
    }
}