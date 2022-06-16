namespace ServiceDesk.Data.Entities
{
    public class Users : BaseEntity
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public int? Active { get; set; }
        public string FullName { get; set; }
        public int DepartmentId { get; set; }
    }
}