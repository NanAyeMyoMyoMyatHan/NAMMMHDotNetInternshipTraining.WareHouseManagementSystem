namespace NAMMMHDotNetInternshipTraining.WareHouseManagementSystem.Models
{
    public class ProductCreateRequestModel
    {
        public string ProductCode { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int MinimumLevel { get; set; }
        public string CreatedBy { get; set; } = null!;
    }

    public class ProductCreateResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = null!;
    }

    public class ProductUpdateRequestModel
    {
        public string ProductCode { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int MinimumLevel { get; set; }
        public bool IsDelete { get; set; }
        public string ModifiedBy { get; set; } = null!;
    }

    public class ProductUpdateResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = null!;
    }

    public class ProductModel
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!; // UI မှာ ပြရလွယ်အောင် ထည့်ပေးထားသည်
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int MinimumLevel { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreatedBy { get; set; } = null!;
        public DateTime? ModifiedDateTime { get; set; }
        public string? ModifiedBy { get; set; }

        
    }
}
