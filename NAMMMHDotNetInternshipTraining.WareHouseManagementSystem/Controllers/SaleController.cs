using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NAMMMHDotNetInternshipTraining.Database.AppDbModels;
using NAMMMHDotNetInternshipTraining.WareHouseManagementSystem.Models;

namespace NAMMMHDotNetInternshipTraining.WareHouseManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly AppDbContext db = new AppDbContext();

        [HttpGet]
        public IActionResult GetSale()
        {
            var lst = db.TblSales
                .Include(x => x.TblSaleItems) // ၁။ အရောင်း Detail ဇယားကို တွဲခေါ်သည်
                .Select(s => new
                {
                    SaleId = s.SaleId,
                    InvoiceNo = s.InvoiceNo, // ဘေလ်နံပါတ်
                    CustomerId = s.CustomerId,
                    TotalAmount = s.TotalAmount,
                    SaleDate = s.SaleDate,
                    Remarks = s.Remarks,
                    CreatedBy = s.CreatedBy,

                    // 🔥 [အဓိကနေရာ] အရောင်းပစ္စည်းစာရင်းကို ၄ ခုတည်းကျန်အောင် ညှစ်ထုတ်ခြင်း
                    TblSaleItems = s.TblSaleItems.Select(si => new
                    {
                        SaleItemId = si.SaleItemId,
                        ProductId = si.ProductId,
                        Quantity = si.Quantity,
                        UnitPrice = si.UnitPrice
                    }).ToList()
                })
                .ToList();

            return Ok(lst);
        }

        [HttpGet("{id}")]
        public IActionResult GetSaleById(int id)
        {
            var item = db.TblSales
                .Include(x => x.TblSaleItems)
                .Select(s => new
                {
                    SaleId = s.SaleId,
                    InvoiceNo = s.InvoiceNo,
                    
                    TotalAmount = s.TotalAmount,
                    SaleDate = s.SaleDate,
                    Remarks = s.Remarks,
                    CreatedBy = s.CreatedBy,

                    // 🔥 [အဓိကနေရာ] အရောင်းပစ္စည်းစာရင်းကို ၄ ခုတည်းကျန်အောင် ညှစ်ထုတ်ခြင်း
                    TblSaleItems = s.TblSaleItems.Select(si => new
                    {
                        SaleItemId = si.SaleItemId,
                        ProductId = si.ProductId,
                        Quantity = si.Quantity,
                        UnitPrice = si.UnitPrice
                    }).ToList()
                })
                .FirstOrDefault(x => x.SaleId == id);

            if (item == null)
            {
                return NotFound("There is no Data");
            }

            return Ok(item);
        }
        [HttpPost]
        public IActionResult CreateSale(SaleCreateRequestModel requestModel)
        {
            // ၁။ အရောင်းဘေလ် ခေါင်းစဉ် (Header) ဆောက်သည်
            var sale = new TblSale
            {
                InvoiceNo = requestModel.InvoiceNo,
                
                Remarks = requestModel.Remarks,
                SaleDate = DateTime.Now,
                CreatedBy = requestModel.CreatedBy,
                TotalAmount = requestModel.SaleItems.Sum(x => x.Quantity * x.UnitPrice)
            };

            // ၂။ ရောင်းမည့် ပစ္စည်းစာရင်းကို Loop ပတ်သည်
            foreach (var item in requestModel.SaleItems)
            {
                // Database ထဲက Product ကို အရင်ရှာသည်
                var product = db.TblProducts.FirstOrDefault(p => p.ProductId == item.ProductId);

                if (product == null)
                    return BadRequest($"Product ID {item.ProductId} ကို ရှာမတွေ့ပါ။");

                // ⚠️ [အရေးကြီးစစ်ဆေးချက်] လက်ကျန်ထက် ပိုရောင်းမရအောင် တားဆီးခြင်း
                if (product.Quantity < item.Quantity)
                {
                    return BadRequest($"{product.ProductName} သည် လက်ကျန် မလုံလောက်ပါ။ (လက်ရှိလက်ကျန်: {product.Quantity} ခုသာရှိသည်)");
                }

                // 🔥 [အဓိကနေရာ] လက်ကျန်ရှိပါက Stock ထဲကနေ အရေအတွက် နှုတ်ပစ်ခြင်း
                product.Quantity -= item.Quantity; // 👈 ရောင်းလိုက်သဖြင့် ပစ္စည်းလက်ကျန်ကို နှုတ်လိုက်သည်

                // အရောင်း Detail ထဲ ထည့်ခြင်း
                sale.TblSaleItems.Add(new TblSaleItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            }

            db.TblSales.Add(sale);
            var result = db.SaveChanges(); // 💡 ဘေလ်လည်းသိမ်းမည်၊ လက်ကျန်လည်း တစ်ပြိုင်နက်တည်း နှုတ်သွားမည်

            return Ok(new SaleCreateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Sale Invoice created successfully and Stock Deducted!" : "Fail to create Sale"
            });
        }
        //[HttpPut("{id}")]
        //public IActionResult UpdateSaleRemarks(int id, SaleUpdateRequestModel requestModel)
        //{
        //    var item = db.TblSales.FirstOrDefault(x => x.SaleId == id);
        //    if (item == null)
        //    {
        //        return NotFound(new SaleUpdateResponseModel
        //        {
        //            IsSuccess = false,
        //            Message = "Sale Invoice not found"
        //        });
        //    }

        //    item.CustomerId = requestModel.CustomerId;
        //    item.Remarks = requestModel.Remarks;

        //    var result = db.SaveChanges();

        //    return Ok(new SaleUpdateResponseModel
        //    {
        //        IsSuccess = result > 0,
        //        Message = result > 0 ? "Invoice updated successfully" : "Fail to update Invoice"
        //    });
        //}

        //[HttpDelete("{id}")]
        //public IActionResult DeleteSale(int id)
        //{
        //    var sale = db.TblSales.FirstOrDefault(x => x.SaleId == id);
        //    if (sale == null)
        //    {
        //        return NotFound(new SaleUpdateResponseModel
        //        {
        //            IsSuccess = false,
        //            Message = "Invoice not found"
        //        });
        //    }

        //    // ၁။ ရောင်းထားသော ပစ္စည်းလိုင်းများကို ရှာဖွေပြီး ကုန်ပစ္စည်းလက်ကျန်ကို ပြန်ပေါင်းထည့်ပေးသည်
        //    var details = db.TblSaleItems.Where(x => x.SaleId == id).ToList();
        //    foreach (var item in details)
        //    {
        //        var product = db.TblProducts.FirstOrDefault(p => p.ProductId == item.ProductId);
        //        if (product != null)
        //        {
        //            product.Quantity += item.Quantity; // ရောင်းထားတာ ဖျက်လိုက်သဖြင့် Stock ပြန်တိုးပေးခြင်း
        //        }
        //        db.TblSaleItems.Remove(item);
        //    }

        //    // ၂။ အရောင်းဘေလ် ခေါင်းစဉ်ကို ဖျက်သည်
        //    db.TblSales.Remove(sale);

        //    var result = db.SaveChanges();

        //    return Ok(new SaleUpdateResponseModel
        //    {
        //        IsSuccess = result > 0,
        //        Message = result > 0 ? "Invoice Deleted Successfully" : "Fail to Delete Invoice"
        //    });
        //}
    }
}
