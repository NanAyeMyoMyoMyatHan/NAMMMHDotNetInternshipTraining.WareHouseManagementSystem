using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NAMMMHDotNetInternshipTraining.Database.AppDbModels;
using NAMMMHDotNetInternshipTraining.WareHouseManagementSystem.Models;

namespace NAMMMHDotNetInternshipTraining.WareHouseManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext db = new AppDbContext();

        [HttpGet]
        public IActionResult GetProduct()
        {
            // Database ထဲမှာရှိသမျှ Product အားလုံးကို ပတ်ပြီး တစ်ခါတည်း အရေအတွက်တွေ တွက်ထုတ်ပေးသွားမှာပါ
            var lst = db.TblProducts
                .Include(x=>x.Category)
                        .Select(p => new
                        {
                            ProductId = p.ProductId,
                            ProductCode = p.ProductCode,
                            ProductName = p.ProductName,
                            Price = p.Price,
                            CurrentStock = p.Quantity, // လက်ရှိ ဆိုင်ထဲက လက်ကျန်

                            CategoryName = p.Category.CategoryName,
                            // 🔥 ၁။ ဒီ Product ကို စုစုပေါင်း ဝယ်ခဲ့သမျှ အရေအတွက်ပေါင်း
                            TotalQuantityPurchased = p.TblPurchaseItems.Sum(pi => (int?)pi.Quantity) ?? 0,

                            // 🔥 ၂။ ဒီ Product ကို စုစုပေါင်း ရောင်းခဲ့ရသမျှ အရေအတွက်ပေါင်း
                            TotalQuantitySold = p.TblSaleItems.Sum(si => (int?)si.Quantity) ?? 0
                        })
                        .ToList();

            return Ok(lst);
        }
        [HttpGet("{id}")]
        public IActionResult GetProductReport(int id)
        {
            // Database ကနေ ပစ္စည်းကို ရှာဖွေပြီး မင်းလိုချင်တဲ့ ပုံစံအတိုင်း ကွက်တိ ဖွဲ့စည်းလိုက်ပါတယ်
            var productReport = db.TblProducts
                .Include (x=> x.Category)
                .Where(p => p.ProductId == id)
                .Select(p => new
                {
                    ProductId = p.ProductId,
                    ProductCode = p.ProductCode,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    CurrentStock = p.Quantity,
                    CategoryName = p.Category.CategoryName,

                    // 🔥 ၁။ အဝယ်စာရင်း Detail ထဲက Quantity များကို အကုန်ပေါင်းပြီး Total ဝယ်ယူမှုကို တွက်ချက်ခြင်း
                    TotalQuantityPurchased = p.TblPurchaseItems.Sum(pi => (int?)pi.Quantity) ?? 0,

                    // 🔥 ၂။ အရောင်းစာရင်း Detail ထဲက Quantity များကို အကုန်ပေါင်းပြီး Total ရောင်းရမှုကို တွက်ချက်ခြင်း
                    TotalQuantitySold = p.TblSaleItems.Sum(si => (int?)si.Quantity) ?? 0
                })
                .FirstOrDefault();

            if (productReport == null)
            {
                return NotFound("Product not found");
            }

            return Ok(productReport);
        }
        [HttpPost]
        public IActionResult CreateProduct(ProductCreateRequestModel requestModel)
        {
            db.TblProducts.Add(new TblProduct
            {
                ProductCode = requestModel.ProductCode,
                ProductName = requestModel.ProductName,
                CategoryId = requestModel.CategoryId,
                Price = requestModel.Price,
                Quantity = 0, // စဆောက်ချင်းမှာ လက်ကျန်ကို ၀ ဟု သတ်မှတ်သည် (Purchase သွင်းမှ တိုးမည်)
                MinimumLevel = requestModel.MinimumLevel,
                IsDelete = false,
                CreatedDateTime = DateTime.Now,
                CreatedBy = requestModel.CreatedBy
            });

            var result = db.SaveChanges();

            return Ok(new ProductCreateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Product created successfully" : "Fail to create Product"
            });
        }
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ProductUpdateRequestModel requestModel)
        {
            var item = db.TblProducts.FirstOrDefault(x => x.ProductId == id);
            if (item == null)
            {
                return NotFound(new ProductUpdateResponseModel
                {
                    IsSuccess = false,
                    Message = "Product not found"
                });
            }

            item.ProductCode = requestModel.ProductCode;
            item.ProductName = requestModel.ProductName;
            item.CategoryId = requestModel.CategoryId;
            item.Price = requestModel.Price;
            item.MinimumLevel = requestModel.MinimumLevel;
            item.IsDelete = requestModel.IsDelete;
            item.ModifiedDateTime = DateTime.Now;
            item.ModifiedBy = requestModel.ModifiedBy;

            var result = db.SaveChanges();

            return Ok(new ProductUpdateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Product updated successfully" : "Fail to update Product"
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var item = db.TblProducts.FirstOrDefault(x => x.ProductId == id);
            if (item == null)
            {
                return NotFound(new ProductUpdateResponseModel
                {
                    IsSuccess = false,
                    Message = "Can not Delete. Not found data"
                });
            }

            db.TblProducts.Remove(item);
            var result = db.SaveChanges();

            return Ok(new ProductUpdateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Delete Successfully" : "Fail to Delete"
            });
        }

        [HttpPatch("{id}")]
        public IActionResult PatchProduct(int id, ProductUpdateRequestModel requestModel)
        {
            var item = db.TblProducts.FirstOrDefault(x => x.ProductId == id);
            if (item == null)
            {
                return NotFound(new ProductUpdateResponseModel
                {
                    IsSuccess = false,
                    Message = "Can not update. Data not found"
                });
            }

            int count = 0;
            if (!string.IsNullOrEmpty(requestModel.ProductCode))
            {
                count++;
                item.ProductCode = requestModel.ProductCode;
            }
            if (!string.IsNullOrEmpty(requestModel.ProductName))
            {
                count++;
                item.ProductName = requestModel.ProductName;
            }
            if (requestModel.CategoryId > 0)
            {
                count++;
                item.CategoryId = requestModel.CategoryId;
            }
            if (requestModel.Price > 0)
            {
                count++;
                item.Price = requestModel.Price;
            }
            if (requestModel.MinimumLevel > 0)
            {
                count++;
                item.MinimumLevel = requestModel.MinimumLevel;
            }

            count++;
            item.IsDelete = requestModel.IsDelete;
            item.ModifiedDateTime = DateTime.Now;
            item.ModifiedBy = requestModel.ModifiedBy;

            var result = db.SaveChanges();
            return Ok(new ProductUpdateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Success" : "Not Success"
            });
        }
    }
}
