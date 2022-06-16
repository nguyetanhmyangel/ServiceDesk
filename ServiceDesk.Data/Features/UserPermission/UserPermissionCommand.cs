namespace ServiceDesk.Data.Features.UserPermission
{
    public class UserPermissionCommand //: BaseEntity
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public int UserId { get; set; }
        public int UserPermission { get; set; }
    }
}