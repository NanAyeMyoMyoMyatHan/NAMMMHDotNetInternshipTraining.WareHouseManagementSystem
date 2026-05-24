namespace NAMMMHDotNetInternshipTraining.WareHouseManagementSystem.Models
{
    public class CustomerCreateRequestModel
    {
        public string CustomerName { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }

    public class CustomerCreateResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = null!;
    }

    public class CustomerUpdateRequestModel
    {
        public string CustomerName { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool IsDelete { get; set; }
    }

    public class CustomerUpdateResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = null!;
    }

    public class CustomerModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
