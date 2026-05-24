using System;
using System.Collections.Generic;

namespace NAMMMHDotNetInternshipTraining.Database.AppDbModels;

public partial class TblPurchase
{
    public int PurchaseId { get; set; }

    public string VoucherNo { get; set; } = null!;

    public int SupplierId { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime PurchaseDate { get; set; }

    public string? Remarks { get; set; }

    public string CreatedBy { get; set; } = null!;

    public virtual TblSupplier Supplier { get; set; } = null!;

    public virtual ICollection<TblPurchaseItem> TblPurchaseItems { get; set; } = new List<TblPurchaseItem>();
}
