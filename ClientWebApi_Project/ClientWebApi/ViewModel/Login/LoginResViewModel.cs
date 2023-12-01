namespace ClientWebApi.ViewModel.Login
{
    public class LoginResViewModel
    {
        public string Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? TokenExpiryTime { get; set; }


    }
}
