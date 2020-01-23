using System;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infraestructure.EntityFramework
{
    public class BenchmarkDBContext : DbContext
    {

        public BenchmarkDBContext(DbContextOptions<BenchmarkDBContext> options)
            : base(options)
        {
            
        }
        
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("TB_PRODUCT");
                
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id)
                    .HasColumnName("Id")
                    .ValueGeneratedNever();
                
                entity.Property(e => e.Name)
                    .HasColumnName("Name")
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
                
                entity.Property(e => e.Description)
                    .HasColumnName("Description")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                    .HasColumnName("Price")
                    .IsRequired();
            });
        }
    }
}