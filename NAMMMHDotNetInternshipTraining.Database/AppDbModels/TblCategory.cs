using System;
using System.Collections.Generic;

namespace NAMMMHDotNetInternshipTraining.Database.AppDbModels;

public partial class TblCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public bool IsDelete { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public virtual ICollection<TblProduct> TblProducts { get; set; } = new List<TblProduct>();
}
