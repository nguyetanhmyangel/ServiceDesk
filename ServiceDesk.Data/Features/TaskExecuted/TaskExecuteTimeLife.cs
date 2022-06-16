namespace ServiceDesk.Data.Features.TaskExecuted
{
    public class TaskExecuteTimeLife
    {
        public int Id { get; set; }
        public int DepartmentIssueId { get; set; }
        public string UserId { get; set; }
        public int Progress { get; set; }
        public string FinishDate { get; set; }
        public string CreateUser { get; set; }
        public string CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public string UpdateDate { get; set; }
        public string Description { get; set; }
        public string LeaderDescription { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int StatusId { get; set; }
        public string Title { get; set; }
        public string EmployeeId { get; set; }
        public string CustomerDescription { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public string EmployeeName { get; set; }
        public string StatusName { get; set; }
        public int DivisionId { get; set; }
        public string CustomerDepartmentName { get; set; }
        public string CustomerDivisionName { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}
