using ServiceDesk.Data.Entities;

namespace ServiceDesk.Data.ViewModels
{
    public class UserPermissionsViewModel //: BaseEntity
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public int UserId { get; set; }
        public int UserPermission { get; set; }
        public string MenuName { get; set; }
        public bool MenuActive { get; set; }
    }
}