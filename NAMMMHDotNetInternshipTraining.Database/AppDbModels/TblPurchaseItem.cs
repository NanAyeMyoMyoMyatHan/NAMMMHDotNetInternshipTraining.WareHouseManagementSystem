using System;
using System.Collections.Generic;

namespace NAMMMHDotNetInternshipTraining.Database.AppDbModels;

public partial class TblPurchaseItem
{
    public int PurchaseItemId { get; set; }

    public int PurchaseId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual TblProduct Product { get; set; } = null!;

    public virtual TblPurchase Purchase { get; set; } = null!;
}
