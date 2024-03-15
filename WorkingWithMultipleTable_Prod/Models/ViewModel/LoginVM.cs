using System.ComponentModel.DataAnnotations;

namespace WorkingWithMultipleTable_Prod.Models.ViewModel
{
    public class LoginVM
    {

        [EmailAddress]
        [Required(ErrorMessage = "Please Enter Email")]
        public string Email { get; set; } = default!;
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter password")]
        public string Password { get; set; } = default!;
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
