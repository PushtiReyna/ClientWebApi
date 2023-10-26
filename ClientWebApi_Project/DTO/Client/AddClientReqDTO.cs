using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTO.Client
{
    public class AddClientReqDTO
    {
        public int Id { get; set; }

        public string Fullname { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public DateTime Dob { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public string Image { get; set; } = null!;

        public string pic { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
