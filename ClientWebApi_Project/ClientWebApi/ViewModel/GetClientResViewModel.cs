using System.ComponentModel.DataAnnotations;

namespace ClientWebApi.ViewModel
{
    public class GetClientResViewModel
    {
        public int Id { get; set; }

        public string Fullname { get; set; }

        public string Gender { get; set; }

        public DateTime Dob { get; set; }

        public string Image { get; set; }

        public string Username { get; set; }

        public string? DocumentName { get; set; }

        public string? Document { get; set; }

    }
}
