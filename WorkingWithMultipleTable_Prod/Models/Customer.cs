using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkingWithMultipleTable_Prod.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string? CustomerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Country { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public int Age { get; set; }
        [NotMapped]
        public bool Active { get; set; }
    }
}
