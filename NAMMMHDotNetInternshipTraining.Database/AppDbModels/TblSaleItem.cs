using System;
using System.Collections.Generic;

namespace NAMMMHDotNetInternshipTraining.Database.AppDbModels;

public partial class TblSaleItem
{
    public int SaleItemId { get; set; }

    public int SaleId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual TblProduct Product { get; set; } = null!;

    public virtual TblSale Sale { get; set; } = null!;
}
