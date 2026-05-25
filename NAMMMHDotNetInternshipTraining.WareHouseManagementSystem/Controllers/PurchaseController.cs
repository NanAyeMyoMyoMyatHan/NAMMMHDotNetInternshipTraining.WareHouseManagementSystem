using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var lst = db.TblPurchases
                .Include(x => x.TblPurchaseItems)
                .Select(p => new
                {
                    PurchaseId = p.PurchaseId,
                    VoucherNo = p.VoucherNo,
                    SupplierId = p.SupplierId,
                    TotalAmount = p.TotalAmount,
                    PurchaseDate = p.PurchaseDate,
                    Remarks = p.Remarks,
                    CreatedBy = p.CreatedBy,
                   
                    TblPurchaseItems = p.TblPurchaseItems.Select(pi => new
                    {
                        PurchaseItemId = pi.PurchaseItemId,
                        ProductId = pi.ProductId,
                        Quantity = pi.Quantity,
                        UnitPrice = pi.UnitPrice
                    }).ToList()

                })
                .ToList();
            return Ok(lst);
        }

        [HttpGet("{id}")]
        public IActionResult GetPurchaseById(int id)
        {
            var item = db.TblPurchases.Include(x=>x.TblPurchaseItems)
                .Select(p => new
                {
                    PurchaseId = p.PurchaseId,
                    VoucherNo = p.VoucherNo,
                    SupplierId = p.SupplierId,
                    TotalAmount = p.TotalAmount,
                    PurchaseDate = p.PurchaseDate,
                    Remarks = p.Remarks,
                    CreatedBy = p.CreatedBy,

                    TblPurchaseItems = p.TblPurchaseItems.Select(pi => new
                    {
                        PurchaseItemId = pi.PurchaseItemId,
                        ProductId = pi.ProductId,
                        Quantity = pi.Quantity,
                        UnitPrice = pi.UnitPrice
                    }).ToList()

                }).FirstOrDefault(x => x.PurchaseId == id)
                ;
            if (item == null)
            {
                return NotFound("There is no Data");
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreatePurchase(PurchaseCreateRequestModel requestModel)
        {
            // ၁။ အဝယ်ဘေလ် ခေါင်းစဉ် (Header) ကို အရင်ဆောက်သည်
            var purchase = new TblPurchase
            {
                VoucherNo = requestModel.VoucherNo,
                SupplierId = requestModel.SupplierId,
                Remarks = requestModel.Remarks,
                PurchaseDate = DateTime.Now,
                CreatedBy = requestModel.CreatedBy,
                TotalAmount = requestModel.PurchaseItems.Sum(x => x.Quantity * x.UnitPrice)
            };

            // ၂။ ပါလာသမျှ အဝယ်ပစ္စည်းလိုင်း (Detail Items) များကို Loop ပတ်သည်
            foreach (var item in requestModel.PurchaseItems)
            {
                purchase.TblPurchaseItems.Add(new TblPurchaseItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });

                // 🔥 [အဓိကနေရာ] Database ထဲက သက်ဆိုင်ရာ Product ကို ရှာပြီး Stock Quantity တိုးပေးခြင်း
                var product = db.TblProducts.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product != null)
                {
                    product.Quantity += item.Quantity; // 👈 မူလလက်ကျန်ထဲကို အသစ်ဝယ်တဲ့ အရေအတွက် ပေါင်းထည့်သည်
                }
            }

            db.TblPurchases.Add(purchase);
            var result = db.SaveChanges(); // 💡 တစ်ခါတည်းနဲ့ ဘေလ်ရော၊ Stock တိုးတာရော အကုန်သိမ်းသွားမည်

            return Ok(new PurchaseCreateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Purchase Voucher created successfully and Stock Updated!" : "Fail to create Purchase"
            });
        }
        //[HttpPut("{id}")]
        //public IActionResult UpdatePurchaseRemarks(int id, PurchaseUpdateRequestModel requestModel)
        //{
        //    var item = db.TblPurchases.FirstOrDefault(x => x.PurchaseId == id);
        //    if (item == null)
        //    {
        //        return NotFound(new PurchaseUpdateResponseModel
        //        {
        //            IsSuccess = false,
        //            Message = "Purchase Voucher not found"
        //        });
        //    }

        //    // Header ၏ Supplier နှင့် မှတ်ချက်ကိုသာ ပြင်ခွင့်ပေးသည်
        //    item.SupplierId = requestModel.SupplierId;
        //    item.Remarks = requestModel.Remarks;

        //    var result = db.SaveChanges();

        //    return Ok(new PurchaseUpdateResponseModel
        //    {
        //        IsSuccess = result > 0,
        //        Message = result > 0 ? "Voucher updated successfully" : "Fail to update Voucher"
        //    });
        //}

        //[HttpDelete("{id}")]
        //public IActionResult DeletePurchase(int id)
        //{
        //    // အဝယ်ဘေလ်ကို ဖျက်ပါက တွဲရက်ပါဝင်သော Items (Detail) များကိုပါ အရင်ဖျက်ပေးရပါမည်
        //    var purchase = db.TblPurchases.FirstOrDefault(x => x.PurchaseId == id);
        //    if (purchase == null)
        //    {
        //        return NotFound(new PurchaseUpdateResponseModel
        //        {
        //            IsSuccess = false,
        //            Message = "Voucher not found"
        //        });
        //    }

        //    // ၁။ သက်ဆိုင်ရာ Detail Items များကို အရင်ရှာပြီး ဖျက်သည်
        //    var details = db.TblPurchaseItems.Where(x => x.PurchaseId == id).ToList();

        //    // 💡 လုပ်ငန်းခွင်တွင် ဘေလ်ဖျက်ပါက သွင်းထားသော Product Stock Quantity ကိုပါ ပြန်နှုတ်ပေးရပါမည်
        //    foreach (var item in details)
        //    {
        //        var product = db.TblProducts.FirstOrDefault(p => p.ProductId == item.ProductId);
        //        if (product != null)
        //        {
        //            product.Quantity -= item.Quantity; // Stock ပြန်နှုတ်ခြင်း
        //        }
        //        db.TblPurchaseItems.Remove(item);
        //    }

        //    // ၂။ Header ကို ဖျက်သည်
        //    db.TblPurchases.Remove(purchase);

        //    var result = db.SaveChanges();

        //    return Ok(new PurchaseUpdateResponseModel
        //    {
        //        IsSuccess = result > 0,
        //        Message = result > 0 ? "Voucher Deleted Successfully" : "Fail to Delete Voucher"
        //    });
        //}
    }
}
