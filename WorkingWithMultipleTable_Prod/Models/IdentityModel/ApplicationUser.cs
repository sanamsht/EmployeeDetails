using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WorkingWithMultipleTable_Prod.Models.IdentityModel
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        
        public DateTime? BirthDate { get; set; }
        public DateTime? CreatedOn { get; set; } 
        public DateTime? ModifiedOn { get; set; } 
        public bool Status { get; set; }


    }
}
