using System;
using System.Collections.Generic;

namespace NAMMMHDotNetInternshipTraining.Database.AppDbModels;

public partial class TblSupplier
{
    public int SupplierId { get; set; }

    public string SupplierName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public bool IsDelete { get; set; }

    public virtual ICollection<TblPurchase> TblPurchases { get; set; } = new List<TblPurchase>();
}
