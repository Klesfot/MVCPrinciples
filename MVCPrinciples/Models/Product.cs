﻿using System.ComponentModel.DataAnnotations;

namespace MVCPrinciples.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        [StringLength(30, MinimumLength = 3)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Required]
        public string? ProductName { get; set; }
        public int SupplierId { get; set; }
        public int CategoryId { get; set; }
        [Required]
        public string? QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public short UnitsInStock { get; set; }
        public short UnitsOnOrder { get; set; }
        public short ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
    }
}
