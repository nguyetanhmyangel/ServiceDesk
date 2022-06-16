using ServiceDesk.Data.Entities;

namespace ServiceDesk.Data.ViewModels
{
    public class LoginViewModel //: BaseEntity
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
}