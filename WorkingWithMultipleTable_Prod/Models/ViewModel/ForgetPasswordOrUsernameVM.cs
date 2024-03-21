
using System.ComponentModel.DataAnnotations;

namespace WorkingWithMultipleTable_Prod.Models.ViewModel
{
    public class ForgetPasswordOrUsernameVM
    {
        [Required] 
        public string Email { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string Token { get; set; }=default!;
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
        [Display(Name ="Confirm Password")]
        [Compare("Password", ErrorMessage ="Password Mismatch")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = default!;

    }
}
