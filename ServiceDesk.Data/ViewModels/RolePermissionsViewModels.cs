using ServiceDesk.Data.Entities;

namespace ServiceDesk.Data.ViewModels
{
    public class RolePermissionsViewModel //: BaseEntity
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }
        public int RolePermission { get; set; }
        public string MenuName { get; set; }
        public bool MenuActive { get; set; }
    }
}