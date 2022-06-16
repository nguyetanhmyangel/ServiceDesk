using ServiceDesk.Data.Entities;
using System;

namespace ServiceDesk.Data.ViewModels
{
    public class IssuesViewModel //: BaseEntity
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CustomerDescription { get; set; }
        public int PriorityId { get; set; }
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
    }
}