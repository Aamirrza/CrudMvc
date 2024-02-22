using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

 namespace MVC.Models
{
    public class tblUser
    {
        [Display(Name = "User Id")]
        public int userId { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username cannot be blank!!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public string? userName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email cannot be blank!!")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Address")]
        public string? userEmail { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password cannot be blank!!")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string? userPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("userPassword", ErrorMessage = "Password and Confirm Password do not match.")]
        public string? confirmPass { get; set; }
    }
}
