using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace walkeasyfinal.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }



    public DbSet<Shoes> Shoes { get; set; }
    public virtual DbSet<Registration> Registrations { get; set; }
    public virtual DbSet<Feedback> Feedbacks { get; set; }

   

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\ProjectModels;Database=walkeasyfinal;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.UserId);  // ✅ UserId is the Primary Key

            entity.ToTable("Feedback");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(f => f.Registration)
                  .WithOne(r => r.Feedback)  // ✅ One-to-One Relationship
                  .HasForeignKey<Feedback>(f => f.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("Registration");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Mobilenumber)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsFixedLength();

            entity.HasOne(r => r.Feedback)
                  .WithOne(f => f.Registration)
                  .HasForeignKey<Feedback>(f => f.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });




        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
