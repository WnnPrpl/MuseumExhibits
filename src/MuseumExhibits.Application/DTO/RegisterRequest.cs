﻿using System.ComponentModel.DataAnnotations;

namespace MuseumExhibits.Application.DTO
{
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
