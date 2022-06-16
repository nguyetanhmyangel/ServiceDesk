using System;

namespace ServiceDesk.Data.Features.Task
{
    public class TaskCommand //: BaseEntity
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public int IssueId { get; set; }
        public int TaskId { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Description { get; set; }
        public string PrivateDescription { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int PriorityId { get; set; }
        public string PriorityName { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime UpdateDate{ get; set; }
        public string UpdateUser { get; set; }
        public DateTime ApproveDate { get; set; }
        public string ApproveUser { get; set; }
        public string UserHandleList { get; set; }
        public int UserDepartmentId { get; set; }

        public int IssueStatusId { get; set; }
        public string IssueStatusName { get; set; }
        public int IssuePriorityId { get; set; }
        public string IssuePriorityName { get; set; }

    }
}