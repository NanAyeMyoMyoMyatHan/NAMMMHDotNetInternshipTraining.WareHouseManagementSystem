using System;
using System.Collections.Generic;

namespace NAMMMHDotNetInternshipTraining.Database.AppDbModels;

public partial class TblSale
{
    public int SaleId { get; set; }

    public string InvoiceNo { get; set; } = null!;

    public int CustomerId { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime SaleDate { get; set; }

    public string? Remarks { get; set; }

    public string CreatedBy { get; set; } = null!;

    public virtual TblCustomer Customer { get; set; } = null!;

    public virtual ICollection<TblSaleItem> TblSaleItems { get; set; } = new List<TblSaleItem>();
}
