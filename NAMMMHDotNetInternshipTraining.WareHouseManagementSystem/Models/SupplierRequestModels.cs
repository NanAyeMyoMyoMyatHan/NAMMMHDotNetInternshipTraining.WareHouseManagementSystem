namespace NAMMMHDotNetInternshipTraining.WareHouseManagementSystem.Models
{
    public class SupplierCreateRequestModel
    {
        public string SupplierName { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool IsDelete { get; set; }
    }

    public class SupplierCreateResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = null!;
    }

    public class SupplierUpdateRequestModel
    {
        public string SupplierName { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool IsDelete { get; set; }
    }

    public class SupplierUpdateResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = null!;
    }

    public class SupplierModel
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool IsDelete { get; set; }
    }
}
