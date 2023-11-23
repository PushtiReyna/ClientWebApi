using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DTO.Client
{
    public class GetClientResDTO
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
