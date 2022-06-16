namespace ServiceDesk.Data.Features.Task
{
    public class TaskTimeLife
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int IssueId { get; set; }

        public int TaskId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int TransferDepartmentId { get; set; }
        public string Description { get; set; }
        public string PrivateDescription { get; set; }
        public string TransferDescription { get; set; }
        public string CustomerDepartmentId { get; set; }
        public string CustomerDepartmentName { get; set; }
        public int CustomerDivisionId { get; set; }
        public string CustomerDivisionName { get; set; }
        public string CustomerDescription { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public string ApproveDate { get; set; }
        public string ApproveUser { get; set; }
        public string UserHandleList { get; set; }
    }
}
