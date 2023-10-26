using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace ClientWebApi.ViewModel
{
    public class AddClientReqViewModel
    {
        public int Id { get; set; }

        public string Fullname { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public DateTime Dob { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public string Image { get; set; } = null!;

        public IFormFile? pic { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
