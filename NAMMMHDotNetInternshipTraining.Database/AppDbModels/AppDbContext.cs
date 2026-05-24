using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NAMMMHDotNetInternshipTraining.Database.AppDbModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblCategory> TblCategories { get; set; }

    public virtual DbSet<TblCustomer> TblCustomers { get; set; }

    public virtual DbSet<TblProduct> TblProducts { get; set; }

    public virtual DbSet<TblPurchase> TblPurchases { get; set; }

    public virtual DbSet<TblPurchaseItem> TblPurchaseItems { get; set; }

    public virtual DbSet<TblSale> TblSales { get; set; }

    public virtual DbSet<TblSaleItem> TblSaleItems { get; set; }

    public virtual DbSet<TblSupplier> TblSuppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=WareHouseManagementSystem;User ID=sa; Password=sasa@123;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Tbl_Cate__19093A0B916209BE");

            entity.ToTable("Tbl_Category");

            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<TblCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Tbl_Cust__A4AE64D89A0075BC");

            entity.ToTable("Tbl_Customer");

            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblProduct>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Tbl_Prod__B40CC6CDDB2334D3");

            entity.ToTable("Tbl_Product");

            entity.HasIndex(e => e.ProductCode, "UQ__Tbl_Prod__2F4E024FFF65F9AC").IsUnique();

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MinimumLevel).HasDefaultValue(5);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProductName)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.HasOne(d => d.Category).WithMany(p => p.TblProducts)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Category");
        });

        modelBuilder.Entity<TblPurchase>(entity =>
        {
            entity.HasKey(e => e.PurchaseId).HasName("PK__Tbl_Purc__6B0A6BBECADF1BB4");

            entity.ToTable("Tbl_Purchase");

            entity.HasIndex(e => e.VoucherNo, "UQ__Tbl_Purc__3AD31D6F468DDC16").IsUnique();

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PurchaseDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VoucherNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Supplier).WithMany(p => p.TblPurchases)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Purchase_Supplier");
        });

        modelBuilder.Entity<TblPurchaseItem>(entity =>
        {
            entity.HasKey(e => e.PurchaseItemId).HasName("PK__Tbl_Purc__B48BB687A1AA3E46");

            entity.ToTable("Tbl_PurchaseItem");

            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.TblPurchaseItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseItem_Product");

            entity.HasOne(d => d.Purchase).WithMany(p => p.TblPurchaseItems)
                .HasForeignKey(d => d.PurchaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PurchaseItem_Purchase");
        });

        modelBuilder.Entity<TblSale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("PK__Tbl_Sale__1EE3C3FFE6D893DD");

            entity.ToTable("Tbl_Sale");

            entity.HasIndex(e => e.InvoiceNo, "UQ__Tbl_Sale__D796B227DD0A3624").IsUnique();

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SaleDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.TblSales)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sale_Customer");
        });

        modelBuilder.Entity<TblSaleItem>(entity =>
        {
            entity.HasKey(e => e.SaleItemId).HasName("PK__Tbl_Sale__C6059401659FBB62");

            entity.ToTable("Tbl_SaleItem");

            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.TblSaleItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleItem_Product");

            entity.HasOne(d => d.Sale).WithMany(p => p.TblSaleItems)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleItem_Sale");
        });

        modelBuilder.Entity<TblSupplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Tbl_Supp__4BE666B4A39F293A");

            entity.ToTable("Tbl_Supplier");

            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SupplierName)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
