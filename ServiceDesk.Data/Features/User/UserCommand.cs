using System;

namespace ServiceDesk.Data.Features.User
{
    [Serializable]
    public class UserCommand //: BaseEntity
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public bool Active { get; set; }
        public string FullName { get; set; }
        public int PositionId { get; set; }
        public int DepartmentId { get; set; }
        public int DivisionId { get; set; }
    }
}