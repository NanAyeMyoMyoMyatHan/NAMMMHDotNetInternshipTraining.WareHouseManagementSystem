using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NAMMMHDotNetInternshipTraining.Database.AppDbModels;
using NAMMMHDotNetInternshipTraining.WareHouseManagementSystem.Models;

namespace NAMMMHDotNetInternshipTraining.WareHouseManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly AppDbContext db = new AppDbContext();

        [HttpGet]
        public IActionResult GetPurchase()
        {
            var lst = db.TblPurchases.ToList();
            return Ok(lst);
        }

        [HttpGet("{id}")]
        public IActionResult GetPurchaseById(int id)
        {
            var item = db.TblPurchases.FirstOrDefault(x => x.PurchaseId == id);
            if (item == null)
            {
                return NotFound("There is no Data");
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreatePurchase(PurchaseCreateRequestModel requestModel)
        {
            // ၁။ Purchase Header Object ကို တည်ဆောက်သည်
            var purchase = new TblPurchase
            {
                VoucherNo = requestModel.VoucherNo,
                SupplierId = requestModel.SupplierId,
                Remarks = requestModel.Remarks,
                PurchaseDate = DateTime.Now,
                CreatedBy = requestModel.CreatedBy,
                TotalAmount = requestModel.PurchaseItems.Sum(x => x.Quantity * x.UnitPrice) // Total ကို Auto တွက်သည်
            };

            // ၂။ Loop ပတ်ပြီး Detail Items များကို Header အောက်ထဲ ထည့်သည်
            foreach (var item in requestModel.PurchaseItems)
            {
                purchase.TblPurchaseItems.Add(new TblPurchaseItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });

                // 💡 လက်တွေ့လုပ်ငန်းခွင်တွင် ပစ္စည်းဝယ်လိုက်သဖြင့် Product Quantity ကိုပါ သွားတိုးပေးရပါမည်
                var product = db.TblProducts.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product != null)
                {
                    product.Quantity += item.Quantity;
                }
            }

            db.TblPurchases.Add(purchase);
            var result = db.SaveChanges(); // ဤနေရာတွင် Header၊ Detail နှင့် Stock တိုးခြင်းအားလုံး တစ်ခါတည်း ဝင်သွားမည်ဖြစ်သည်

            return Ok(new PurchaseCreateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Purchase Voucher created successfully" : "Fail to create Purchase"
            });
        }
        [HttpPut("{id}")]
        public IActionResult UpdatePurchaseRemarks(int id, PurchaseUpdateRequestModel requestModel)
        {
            var item = db.TblPurchases.FirstOrDefault(x => x.PurchaseId == id);
            if (item == null)
            {
                return NotFound(new PurchaseUpdateResponseModel
                {
                    IsSuccess = false,
                    Message = "Purchase Voucher not found"
                });
            }

            // Header ၏ Supplier နှင့် မှတ်ချက်ကိုသာ ပြင်ခွင့်ပေးသည်
            item.SupplierId = requestModel.SupplierId;
            item.Remarks = requestModel.Remarks;

            var result = db.SaveChanges();

            return Ok(new PurchaseUpdateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Voucher updated successfully" : "Fail to update Voucher"
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePurchase(int id)
        {
            // အဝယ်ဘေလ်ကို ဖျက်ပါက တွဲရက်ပါဝင်သော Items (Detail) များကိုပါ အရင်ဖျက်ပေးရပါမည်
            var purchase = db.TblPurchases.FirstOrDefault(x => x.PurchaseId == id);
            if (purchase == null)
            {
                return NotFound(new PurchaseUpdateResponseModel
                {
                    IsSuccess = false,
                    Message = "Voucher not found"
                });
            }

            // ၁။ သက်ဆိုင်ရာ Detail Items များကို အရင်ရှာပြီး ဖျက်သည်
            var details = db.TblPurchaseItems.Where(x => x.PurchaseId == id).ToList();

            // 💡 လုပ်ငန်းခွင်တွင် ဘေလ်ဖျက်ပါက သွင်းထားသော Product Stock Quantity ကိုပါ ပြန်နှုတ်ပေးရပါမည်
            foreach (var item in details)
            {
                var product = db.TblProducts.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product != null)
                {
                    product.Quantity -= item.Quantity; // Stock ပြန်နှုတ်ခြင်း
                }
                db.TblPurchaseItems.Remove(item);
            }

            // ၂။ Header ကို ဖျက်သည်
            db.TblPurchases.Remove(purchase);

            var result = db.SaveChanges();

            return Ok(new PurchaseUpdateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Voucher Deleted Successfully" : "Fail to Delete Voucher"
            });
        }
    }
}
