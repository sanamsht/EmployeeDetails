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
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime? BirthDate { get; set; }
        public DateTime? CreatedOn { get; set; } 
        public DateTime? ModifiedOn { get; set; } 
        [Display(Name = "Active")]
        public bool Status { get; set; }
        public string Username { get; set; } = default!;
    }
}
