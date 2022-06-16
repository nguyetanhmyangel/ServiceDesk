namespace ServiceDesk.Data.Features.Login
{
    public class LoginResponse 
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HrPassword { get; set; }
        public bool Remember { get; set; }
    }
}