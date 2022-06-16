namespace ServiceDesk.Data.Features.Employee
{
    public class EmployeeResponse
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        public string Password { get; set; }
        public string HrPassword { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentRussianName { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public string DivisionRussianName { get; set; }
        public int UserId { get; set; }
        public int NumExecute { get; set; }
    }
}