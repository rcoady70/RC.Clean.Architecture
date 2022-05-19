namespace RC.CA.Application.Dto.Authentication
{
    public class RefreshLoginRequest
    {
        public string UserName { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
