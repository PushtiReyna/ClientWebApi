using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Login
{
    public class LoginResDTO
    {

        public string Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? TokenExpiryTime { get; set; }


    }
}
