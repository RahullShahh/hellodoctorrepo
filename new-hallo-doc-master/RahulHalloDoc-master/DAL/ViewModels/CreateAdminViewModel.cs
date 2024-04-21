using DAL.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DAL.ViewModels
{
    public class CreateAdminViewModel
    {
        public List<Region> Regions { get; set; }

        public string? UserName { get; set; }

        public int? state { get; set; }

        public List<Role>? roles { get; set; }


        public string? Role { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, one special character, and be at least 8 characters long.")]
        public string? Password { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Confirm Email is required")]
        [DataType(DataType.EmailAddress)]
        [Compare("Email", ErrorMessage = "The email and confirmation email do not match.")]
        public string? ConfirmEmail { get; set; }

        public string? PhoneNumAspNetUsers { get; set; }

        public string? Address1 { get; set; }
        public string? Address2 { get; set; }

        public string? City { get; set; }

        public string? zip { get; set; }

        public string? MobileNumAdmin { get; set; }

        public int? SelectedStateId { get; set; }
        public List<int>? SelectedRegions { get; set; }

        public List<CheckboxList_model>? statesForChecked { get; set; }
    }
    public class CheckboxList_model
    {
        public int? Value { get; set; }
        public Boolean? Selected { get; set; }
    }
}
