using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AnswerPoortal.Models
{
    public class RegistrationViewModel
    {

        public int id { get; set; }

        [Required]
        [Display(Name = "Name", Prompt = "Name")]
        [StringLength(20)]
        public string name { get; set; }

        [Required]
        [Display(Name = "Surname", Prompt = "Surname")]
        [StringLength(20)]
        public string surname { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Email")]
        [StringLength(50)]
        public string email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", Prompt = "Password")]
        [StringLength(100)]
        public string password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        public string repeatpassword { get; set; }

    }
}