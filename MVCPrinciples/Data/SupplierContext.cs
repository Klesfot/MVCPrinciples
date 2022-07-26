﻿using Microsoft.EntityFrameworkCore;

namespace MVCPrinciples.Data
{
    public class SupplierContext : DbContext
    {
        public SupplierContext(DbContextOptions<SupplierContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Models.Supplier> Suppliers { get; set; }
    }
}