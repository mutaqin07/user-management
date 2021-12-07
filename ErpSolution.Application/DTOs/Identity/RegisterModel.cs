using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ErpSolution.Application.DTOs.Identity
{
    public class RegisterModel
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
        [Required]
        public string Password { get; set; }
        public byte[] ProfilePicture { get; set; }
        [Required]
        public bool EmailConfirmed { get; set; }
        public string Id { get; set; }
    }
}
