namespace SP2.Models
{
    public class LogInRequest
    {
        public string Username { get; set; } = "GOLOGS";
        public string Password { get; set; } = "123";
        public string PhoneNumber { get; set; } = "081230943248";
    }

    public class LogOutRequest
    {
        public string SessionId { get; set; }
    }
}