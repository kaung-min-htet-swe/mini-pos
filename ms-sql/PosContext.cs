using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ms_sql;

public partial class PosContext : DbContext
{
    public PosContext()
    {
    }

    public PosContext(DbContextOptions<PosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Admins__3214EC07460A85C9");

            entity.HasIndex(e => e.Username, "UQ__Admins__536C85E4B30DD322").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Admins__A9D105343D5A6346").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("Cashier");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
            
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC071B4B77BC");

            entity.HasIndex(e => e.Id, "IX_Categories_Active").HasFilter("([DeletedAt] IS NULL)");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07120F8994");

            entity.HasIndex(e => e.Id, "IX_Customers_Active").HasFilter("([DeletedAt] IS NULL)");

            entity.HasIndex(e => e.Email, "IX_Customers_Email");

            entity.HasIndex(e => e.PhoneNumber, "IX_Customers_PhoneNumber");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
            
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC074E28C14D");

            entity.HasIndex(e => e.CustomerId, "IX_Orders_CustomerId");

            entity.HasIndex(e => e.OrderDate, "IX_Orders_OrderDate").IsDescending();

            entity.HasIndex(e => e.ProcessedById, "IX_Orders_ProcessedById");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.OrderDate).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Orders_Customers");

            entity.HasOne(d => d.ProcessedBy).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ProcessedById)
                .HasConstraintName("FK_Orders_Admins");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
            
            entity.HasKey(e => e.Id).HasName("PK__OrderIte__3214EC07B51366B4");

            entity.HasIndex(e => e.OrderId, "IX_OrderItems_OrderId");

            entity.HasIndex(e => e.ProductId, "IX_OrderItems_ProductId");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderItems_Orders");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Products");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);
            
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC07D91059C7");

            entity.HasIndex(e => e.CategoryId, "IX_Products_Active").HasFilter("([DeletedAt] IS NULL)");

            entity.HasIndex(e => e.CategoryId, "IX_Products_CategoryId");

            entity.HasIndex(e => e.Name, "IX_Products_Name");

            entity.HasIndex(e => e.Sku, "UQ__Products__CA1ECF0DDF49614F").IsUnique().HasFilter("([DeletedAt] IS NULL)");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .HasColumnName("SKU");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Categories");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasQueryFilter(e => e.DeletedAt == null);

            entity.HasKey(e => e.Id).HasName("PK__ProductI__3214EC070CB3942C");

            entity.HasIndex(e => e.ProductId, "IX_ProductImages_Active").HasFilter("([DeletedAt] IS NULL)");

            entity.HasIndex(e => e.ProductId, "IX_ProductImages_ProductId");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ImageUrl).HasMaxLength(2048);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductImages_Products");
        });
        
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
