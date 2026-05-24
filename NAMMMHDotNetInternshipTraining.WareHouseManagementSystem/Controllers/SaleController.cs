using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var lst = db.TblSales.ToList();
            return Ok(lst);
        }

        [HttpGet("{id}")]
        public IActionResult GetSaleById(int id)
        {
            var item = db.TblSales.FirstOrDefault(x => x.SaleId == id);
            if (item == null)
            {
                return NotFound("There is no Data");
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateSale(SaleCreateRequestModel requestModel)
        {
            var sale = new TblSale
            {
                InvoiceNo = requestModel.InvoiceNo,
                CustomerId = requestModel.CustomerId,
                Remarks = requestModel.Remarks,
                SaleDate = DateTime.Now,
                CreatedBy = requestModel.CreatedBy,
                TotalAmount = requestModel.SaleItems.Sum(x => x.Quantity * x.UnitPrice)
            };

            foreach (var item in requestModel.SaleItems)
            {
                // ရောင်းမည့် Product ၏ လက်ကျန်ကို စစ်ဆေးသည်
                var product = db.TblProducts.FirstOrDefault(p => p.ProductId == item.ProductId);

                if (product == null)
                    return BadRequest($"Product ID {item.ProductId} not found.");

                if (product.Quantity < item.Quantity)
                    return BadRequest($"{product.ProductName} has insufficient stock. Current: {product.Quantity}");

                // လက်ကျန် နှုတ်ခြင်း
                product.Quantity -= item.Quantity;

                // Sale Detail ထဲ ထည့်ခြင်း
                sale.TblSaleItems.Add(new TblSaleItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            }

            db.TblSales.Add(sale);
            var result = db.SaveChanges();

            return Ok(new SaleCreateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Sale Invoice created successfully" : "Fail to create Sale"
            });
        }
        [HttpPut("{id}")]
        public IActionResult UpdateSaleRemarks(int id, SaleUpdateRequestModel requestModel)
        {
            var item = db.TblSales.FirstOrDefault(x => x.SaleId == id);
            if (item == null)
            {
                return NotFound(new SaleUpdateResponseModel
                {
                    IsSuccess = false,
                    Message = "Sale Invoice not found"
                });
            }

            item.CustomerId = requestModel.CustomerId;
            item.Remarks = requestModel.Remarks;

            var result = db.SaveChanges();

            return Ok(new SaleUpdateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Invoice updated successfully" : "Fail to update Invoice"
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSale(int id)
        {
            var sale = db.TblSales.FirstOrDefault(x => x.SaleId == id);
            if (sale == null)
            {
                return NotFound(new SaleUpdateResponseModel
                {
                    IsSuccess = false,
                    Message = "Invoice not found"
                });
            }

            // ၁။ ရောင်းထားသော ပစ္စည်းလိုင်းများကို ရှာဖွေပြီး ကုန်ပစ္စည်းလက်ကျန်ကို ပြန်ပေါင်းထည့်ပေးသည်
            var details = db.TblSaleItems.Where(x => x.SaleId == id).ToList();
            foreach (var item in details)
            {
                var product = db.TblProducts.FirstOrDefault(p => p.ProductId == item.ProductId);
                if (product != null)
                {
                    product.Quantity += item.Quantity; // ရောင်းထားတာ ဖျက်လိုက်သဖြင့် Stock ပြန်တိုးပေးခြင်း
                }
                db.TblSaleItems.Remove(item);
            }

            // ၂။ အရောင်းဘေလ် ခေါင်းစဉ်ကို ဖျက်သည်
            db.TblSales.Remove(sale);

            var result = db.SaveChanges();

            return Ok(new SaleUpdateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Invoice Deleted Successfully" : "Fail to Delete Invoice"
            });
        }
    }
}
