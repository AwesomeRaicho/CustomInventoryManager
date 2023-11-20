using Entities.Sold;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class AllDbContext : DbContext
    {
        public DbSet<Clothes> Clothes { get; set; }
        public DbSet<Costume> Costumes{ get; set; }
        public DbSet<Product> Products{ get; set; }

        //Archive Tables
        public DbSet<SoldClothes> SoldClothes { get; set; }
        public DbSet<SoldCostume> SoldCostumes { get; set; }
        public DbSet<SoldProduct> SoldProducts { get; set; }




        public AllDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
