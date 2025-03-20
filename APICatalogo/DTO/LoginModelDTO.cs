﻿using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTO;

public class LoginModelDTO
{
    [Required(ErrorMessage = "User name is required")]
    public string? userName { get; set; }

    [Required(ErrorMessage = "pasword is required")]
    public string? Password { get; set; }
}
