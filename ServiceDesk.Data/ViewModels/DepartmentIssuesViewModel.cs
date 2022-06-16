using ServiceDesk.Data.Entities;
using System;

namespace ServiceDesk.Data.ViewModels
{
    public class DepartmentIssuesViewModel //: BaseEntity
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int IssueId { get; set; }       
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TransferDepartmentId { get; set; }
        public string Description { get; set; }
        public string RequestDepartmentId { get; set; }
        public string RequestDepartmentName { get; set; }
        public int RequestDivisionId { get; set; }
        public string RequestDivisionName { get; set; }
        public string RequestDescription { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime UpdateDate{ get; set; }
        public string UpdateUser { get; set; }
        public DateTime ApproveDate { get; set; }
        public string ApproveUser { get; set; }
        public string UserHandleList { get; set; }
    }
}