using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Client
{
    public class GetClientReqDTO
    {
        public int Id { get; set; }

        public string Fullname { get; set; }

        public string Gender { get; set; }

        public DateTime Dob { get; set; }

        public string Image { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
