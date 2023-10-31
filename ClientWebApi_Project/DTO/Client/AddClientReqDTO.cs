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

        public dynamic Image { get; set; } 

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
