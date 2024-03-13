using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WorkingWithMultipleTable_Prod.Models.Combined
{
    public class EmployeeDepartmentModel
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage ="Please Enter First Name")]
        public string FirstName { get; set; } = default!;
        [Required(ErrorMessage = "Please Enter First Name")]
        public string? MiddleName { get; set; }
        [Required(ErrorMessage = "Please Enter First Name")]
        public string? LastName { get; set; }
       
        public string FullName { get
            {
                return FirstName + " " + MiddleName + " " + LastName;
            }
        }
        [Required(ErrorMessage = "Please Select Gender")]
        public string? Gender { get; set; }
        [Required(ErrorMessage = "Please Select Department")]
        public int DepartmentId { get; set; }   
        public string DepartmentName { get; set; } = default!;
        public string DepartmentCode { get; set; } = default!;

    }
}
