﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Client
{
    public class RefreshResDTO
    {
        public string Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}