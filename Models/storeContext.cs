﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace online_store.Models
{
    public partial class storeContext : DbContext
    {
        public storeContext()
        {
        }

        public storeContext(DbContextOptions<storeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Shipper> Shippers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;database=store;user=root;password=Jrichchild13!", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.37-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("latin1_swedish_ci")
                .HasCharSet("latin1");

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customers");

                entity.HasCharSet("utf8mb4")
                    .UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("customer_id");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .HasColumnName("address");

                entity.Property(e => e.BirthDate).HasColumnName("birth_date");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .HasColumnName("city");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .HasColumnName("last_name");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .HasColumnName("phone");

                entity.Property(e => e.Points)
                    .HasColumnType("int(11)")
                    .HasColumnName("points");

                entity.Property(e => e.State)
                    .HasMaxLength(2)
                    .HasColumnName("state")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.HasCharSet("utf8mb4")
                    .UseCollation("utf8mb4_general_ci");

                entity.HasIndex(e => e.CustomerId, "fk_orders_customers_idx");

                entity.HasIndex(e => e.Status, "fk_orders_order_statuses_idx");

                entity.HasIndex(e => e.ShipperId, "fk_orders_shippers_idx");

                entity.Property(e => e.OrderId)
                    .HasColumnType("int(11)")
                    .HasColumnName("order_id");

                entity.Property(e => e.Comments)
                    .HasMaxLength(2000)
                    .HasColumnName("comments");

                entity.Property(e => e.CustomerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("customer_id");

                entity.Property(e => e.OrderDate).HasColumnName("order_date");

                entity.Property(e => e.ShippedDate).HasColumnName("shipped_date");

                entity.Property(e => e.ShipperId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("shipper_id");

                entity.Property(e => e.Status)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("status")
                    .HasDefaultValueSql("'1'");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_orders_customers");

                entity.HasOne(d => d.Shipper)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ShipperId)
                    .HasConstraintName("fk_orders_shippers");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_orders_order_statuses");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ProductId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("order_items");

                entity.HasCharSet("utf8mb4")
                    .UseCollation("utf8mb4_general_ci");

                entity.HasIndex(e => e.ProductId, "fk_order_items_products_idx");

                entity.Property(e => e.OrderId)
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("order_id");

                entity.Property(e => e.ProductId)
                    .HasColumnType("int(11)")
                    .HasColumnName("product_id");

                entity.Property(e => e.Quantity)
                    .HasColumnType("int(11)")
                    .HasColumnName("quantity");

                entity.Property(e => e.UnitPrice)
                    .HasPrecision(4, 2)
                    .HasColumnName("unit_price");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_order_items_orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_order_items_products");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.ToTable("order_statuses");

                entity.HasCharSet("utf8mb4")
                    .UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.OrderStatusId)
                    .HasColumnType("tinyint(4)")
                    .ValueGeneratedNever()
                    .HasColumnName("order_status_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products");

                entity.HasCharSet("utf8mb4")
                    .UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.ProductId)
                    .HasColumnType("int(11)")
                    .HasColumnName("product_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.QuantityInStock)
                    .HasColumnType("int(11)")
                    .HasColumnName("quantity_in_stock");

                entity.Property(e => e.UnitPrice)
                    .HasPrecision(4, 2)
                    .HasColumnName("unit_price");
            });

            modelBuilder.Entity<Shipper>(entity =>
            {
                entity.ToTable("shippers");

                entity.HasCharSet("utf8mb4")
                    .UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.ShipperId)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("shipper_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
