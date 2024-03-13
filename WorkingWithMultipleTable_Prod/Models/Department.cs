using System.ComponentModel.DataAnnotations;

namespace WorkingWithMultipleTable_Prod.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = default!;
        public string DepartmentCode { get; set;} = default!;
    }
}
