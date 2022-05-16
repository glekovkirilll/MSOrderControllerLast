using Microsoft.EntityFrameworkCore;
using MSOC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSOC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Order> Orders { get; set; }
    }

}
