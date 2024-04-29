using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;
using SurtiPlus.Models;


namespace SurtiPlus.Models
{
    public partial class SurtiPlusContext : DbContext
    {
        public SurtiPlusContext()
        {
        }

        public SurtiPlusContext(DbContextOptions<SurtiPlusContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Adminv> Admin { get; set; } = null!;
        public virtual DbSet<Productv> Product { get; set; } = null!;
        public virtual DbSet<Userv> User { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) 
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Adminv>(entity =>
            {
                entity.Property(e => e.AdminPassword)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Productv>(entity =>
            {
                entity.Property(e => e.AdminNameId)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.ProductType)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Userv>(entity =>
            {
                entity.Property(e => e.AdminNameId)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });
            
        }

    }
}
