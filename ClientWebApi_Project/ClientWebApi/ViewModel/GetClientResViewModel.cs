﻿using System.ComponentModel.DataAnnotations;

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

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
