namespace NAMMMHDotNetInternshipTraining.WareHouseManagementSystem.Models
{
    public class CategoryCreateRequestModel
    {
        public string CategoryName { get; set; } = null!;
        public bool IsDelete { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }

    public class CategoryCreateResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = null!;
    }

    public class CategoryUpdateRequestModel
    {
        public string CategoryName { get; set; } = null!;
        public bool IsDelete { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
    public class CategoriesUpdateResponseModel
    {
        public CategoryModel Category { get; set; }

        public bool IsSuccess { get; set; }
        public string Message { get; set; } = null!;
    }
    public class CategoryModel
    {
        public string CategoryName { get; set; } = null!;
        public bool IsDelete { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public List<CategoryProductName>Products { get; set; } = new List<CategoryProductName>();
    }

    public class CategoryProductName
    {
        
        public string ProductName { get; set; } = null!;
        
    }


}
