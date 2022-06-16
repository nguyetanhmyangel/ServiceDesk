using System;

namespace ServiceDesk.Data.Features.Issue
{
    public class IssuesResponse //: BaseEntity
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int Rating { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime ApproveDate { get; set; }
        public string ApproveUser { get; set; }
        public int TagId { get; set; }
        public string TagName { get; set; }
        public string Reason { get; set; }
        public string DispatchNumber { get; set; }
        public bool OwnerCancel { get; set; }
        public string DispatchPath { get; set; }
        public string Review { get; set; }
        public string CustomerDescription { get; set; }
        public int PriorityId { get; set; }
    }
}