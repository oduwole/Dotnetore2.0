using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
//using DotnetCore2._0.Infrastrucutre.Entities;

namespace DotnetCore2.Infrastrucutre.Data.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }
        //public DbSet<Boards> boards { get; set; }
        //public DbSet<Product> Products { get; set; }
        //public DbSet<Order> Orders { get; set; }
        //public DbSet<OrderDetail> OrderDetails { get; set; }

        //public DbSet<Category> Categories { get; set; }
        //public DbSet<Cart> Cart { get; set; }
        //public DbSet<WishList> WishList { get; set; }

        //public DbSet<ProductImages> ProductImages { get; set; }
        //public DbSet<UserProfile> ÙserProfile { get; set; }
        //public DbSet<PaymentMethods> PaymentMethods { get; set; }
        //public DbSet<Currency> Currency { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed. 

            //modelBuilder.Entity<Boards>().HasKey(u => new { u.name, u.owner, u.type });
        }
    }
}
