namespace ServiceDesk.Data.Features.Issue
{
    public class IssueTimeLifeResponse
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string FullName { get; set; }
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
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public string ApproveDate { get; set; }
        public string ApproveUser { get; set; }
        public string DispatchPath { get; set; }
        public string Review { get; set; }
        public string CustomerDescription { get; set; }
        public int PriorityId { get; set; }
    }
}
