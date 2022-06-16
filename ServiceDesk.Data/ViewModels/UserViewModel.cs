using System;
using ServiceDesk.Data.Entities;

namespace ServiceDesk.Data.ViewModels
{
    [Serializable]
    public class UserViewModel //: BaseEntity
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Active { get; set; }
        public string FullName { get; set; }
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
    }
}