namespace NAMMMHDotNetInternshipTraining.WareHouseManagementSystem.Models
{
    public class SaleCreateRequestModel
    {
        public string InvoiceNo { get; set; } = null!;
        public int CustomerId { get; set; }
        public string? Remarks { get; set; }
        public string CreatedBy { get; set; } = null!;
        public List<SaleItemRequestModel> SaleItems { get; set; } = new List<SaleItemRequestModel>();
    }

    public class SaleItemRequestModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class SaleCreateResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = null!;
    }

    public class SaleUpdateRequestModel
    {
        public int CustomerId { get; set; }
        public string? Remarks { get; set; }
    }

    public class SaleUpdateResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = null!;
    }

    public class SaleModel
    {
        public int SaleId { get; set; }
        public string InvoiceNo { get; set; } = null!;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public DateTime SaleDate { get; set; }
        public string? Remarks { get; set; }
        public string CreatedBy { get; set; } = null!;
        public List<SaleItemModel> SaleItems { get; set; } = new List<SaleItemModel>();
    }

    public class SaleItemModel
    {
        public int SaleItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
