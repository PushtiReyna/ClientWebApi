using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace ClientWebApi.ViewModel.Client
{
    public class AddClientReqViewModel
    {
        public string Fullname { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public DateTime Dob { get; set; }

        public IFormFile Image { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
        public decimal? Ctc { get; set; }

        public DateTime? JoiningDate { get; set; }

        public int? ProbationPeriod { get; set; }
    }
}
