namespace ServiceDesk.Data.Entities
{
    public class RolePermissions : BaseEntity
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }
        public int RolePermission { get; set; }
    }
}