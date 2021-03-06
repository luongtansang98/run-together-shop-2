using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RegistrationAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.Models
{
	public class AuthenticationContext: IdentityDbContext
	{
        public AuthenticationContext(DbContextOptions options):base(options)
		{
            
		}
		public DbSet<ApplicationUser> ApplicationUsers { get; set; }
		public DbSet<Position> Positions { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<PromotionType> PromotionTypes { get; set; }
		public DbSet<Promotion> Promotions { get; set; }
		public DbSet<PromotionProduct> PromotionProducts { get; set; }

        public DbQuery<ProductQueryDTO> ProductQueries { get; set; }

        public override int SaveChanges()
        {
            this.ChangeTracker.DetectChanges();
            var added = this.ChangeTracker.Entries()
                        .Where(t => t.State == EntityState.Added)
                        .Select(t => t.Entity)
                        .ToArray();

            foreach (var entity in added)
            {
                if (entity is BaseEntity)
                {
                    var track = entity as BaseEntity;
                    track.CreatedAt = DateTime.Now;
                    track.UpdatedAt = DateTime.Now;
                }
            }

            var modified = this.ChangeTracker.Entries()
                        .Where(t => t.State == EntityState.Modified)
                        .Select(t => t.Entity)
                        .ToArray();

            foreach (var entity in modified)
            {
                if (entity is BaseEntity)
                {
                    var track = entity as BaseEntity;
                    track.UpdatedAt = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PromotionType>().HasData(
                new PromotionType() { Id = 1, CreatedAt = DateTime.Now, Name = "Phần trăm", Description = "Giảm giá theo phần trăm" },
                new PromotionType() { Id = 2, Name = "Giảm tiền", Description = "Trừ theo giá tiền" });
        }
    }
}
