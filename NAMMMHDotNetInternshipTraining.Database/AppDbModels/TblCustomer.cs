using System;
using System.Collections.Generic;

namespace NAMMMHDotNetInternshipTraining.Database.AppDbModels;

public partial class TblCustomer
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public bool IsDelete { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public virtual ICollection<TblSale> TblSales { get; set; } = new List<TblSale>();
}
