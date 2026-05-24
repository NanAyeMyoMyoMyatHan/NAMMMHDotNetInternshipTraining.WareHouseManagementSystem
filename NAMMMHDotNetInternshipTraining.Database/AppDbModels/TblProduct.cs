using System;
using System.Collections.Generic;

namespace NAMMMHDotNetInternshipTraining.Database.AppDbModels;

public partial class TblProduct
{
    public int ProductId { get; set; }

    public string ProductCode { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public int CategoryId { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public int MinimumLevel { get; set; }

    public bool IsDelete { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime? ModifiedDateTime { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual TblCategory Category { get; set; } = null!;

    public virtual ICollection<TblPurchaseItem> TblPurchaseItems { get; set; } = new List<TblPurchaseItem>();

    public virtual ICollection<TblSaleItem> TblSaleItems { get; set; } = new List<TblSaleItem>();
}
