
using System.ComponentModel.DataAnnotations;

namespace WorkingWithMultipleTable_Prod.Models.ViewModel
{
    public class RegisterVM
    {
        [EmailAddress]
        [Required(ErrorMessage = "Please Enter Email")]
        public string Email { get; set; } = default!;
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Please enter password")]
        public string Password { get; set; } = default!;
        [Display(Name ="Confirm Password")]
        [Required(ErrorMessage ="Please enter confirm password")]
        [Compare("Password", ErrorMessage ="Password Mismatch")]
        public string ConfirmPassword { get; set; } = default!;
    }
}
