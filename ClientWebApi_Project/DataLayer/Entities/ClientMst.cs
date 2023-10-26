﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities;

public partial class ClientMst
{
    public int Id { get; set; }

    public string Fullname { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public DateTime Dob { get; set; }

    public string Image { get; set; } = null!;

    //[NotMapped]
    //public IFormFile Picture { get; set; }  
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool IsActive { get; set; }

    public bool IsDelete { get; set; }

    public bool CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool UpdateBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
