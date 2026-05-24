namespace NAMMMHDotNetInternshipTraining.WareHouseManagementSystem.Models
{
    public class PurchaseCreateRequestModel
    {
        public string VoucherNo { get; set; } = null!;
        public int SupplierId { get; set; }
        public string? Remarks { get; set; }
        public string CreatedBy { get; set; } = null!;
        public List<PurchaseItemRequestModel> PurchaseItems { get; set; } = new List<PurchaseItemRequestModel>();
    }

    public class PurchaseItemRequestModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class PurchaseCreateResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = null!;
    }

    // 💡 Transaction ဘေလ်များမှာ ခေါင်းစဉ်ကို ပြင်လေ့မရှိဘဲ လိုအပ်ပါက Delete/Void သာ လုပ်လေ့ရှိ၍
    // UpdateRequest ထဲတွင် Remarks သို့မဟုတ် SupplierId ပြောင်းလဲခြင်းလောက်သာ ထားရှိပါသည်။
    public class PurchaseUpdateRequestModel
    {
        public int SupplierId { get; set; }
        public string? Remarks { get; set; }
    }

    public class PurchaseUpdateResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = null!;
    }

    public class PurchaseModel
    {
        public int PurchaseId { get; set; }
        public string VoucherNo { get; set; } = null!;
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string? Remarks { get; set; }
        public string CreatedBy { get; set; } = null!;
        public List<PurchaseItemModel> PurchaseItems { get; set; } = new List<PurchaseItemModel>();
    }

    public class PurchaseItemModel
    {
        public int PurchaseItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
