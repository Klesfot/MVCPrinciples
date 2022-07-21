using System.ComponentModel.DataAnnotations;

namespace MVCPrinciples.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierID { get; set; }
        public string? CompanyName { get; set; }
    }
}
