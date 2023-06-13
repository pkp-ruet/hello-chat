﻿using System.ComponentModel.DataAnnotations;

namespace HelloChat.Models;

public class LoginRequest
{
    [Required] 
    public string Username { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}