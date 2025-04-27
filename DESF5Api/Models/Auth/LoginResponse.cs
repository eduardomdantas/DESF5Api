namespace DESF5Api.Models.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime ExpiresIn { get; set; }
    }
}