namespace ClientWebApi.ViewModel.Client
{
    public class UpdateClientReqViewModel
    {
        public int Id { get; set; }

        public string Fullname { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public DateTime Dob { get; set; }

        public IFormFile Image { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
