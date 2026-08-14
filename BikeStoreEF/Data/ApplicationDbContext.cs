using System;
using System.Collections.Generic;
using BikeStoreEF.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeStoreEF.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Staff> Staffs { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=bikestore;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__brands__5E5A8E274DEEE815");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__categori__D54EE9B4584ADE64");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__customer__CD65CB85FF1181F5");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__orders__4659622926D37D47");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__orders__customer__6D0D32F4");

            entity.HasOne(d => d.Staff).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__orders__staff_id__6EF57B66");

            entity.HasOne(d => d.Store).WithMany(p => p.Orders).HasConstraintName("FK__orders__store_id__6E01572D");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ItemId }).HasName("PK__order_it__837942D4E2E18098");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems).HasConstraintName("FK__order_ite__order__72C60C4A");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems).HasConstraintName("FK__order_ite__produ__73BA3083");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__products__47027DF5551BBC0E");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products).HasConstraintName("FK__products__brand___619B8048");

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasConstraintName("FK__products__catego__60A75C0F");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__staffs__1963DD9C81FB5893");

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager).HasConstraintName("FK__staffs__manager___6A30C649");

            entity.HasOne(d => d.Store).WithMany(p => p.Staff).HasConstraintName("FK__staffs__store_id__693CA210");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => new { e.StoreId, e.ProductId }).HasName("PK__stocks__E68284D3CCF13E1D");

            entity.HasOne(d => d.Product).WithMany(p => p.Stocks).HasConstraintName("FK__stocks__product___778AC167");

            entity.HasOne(d => d.Store).WithMany(p => p.Stocks).HasConstraintName("FK__stocks__store_id__76969D2E");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("PK__stores__A2F2A30C8158DC0C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
