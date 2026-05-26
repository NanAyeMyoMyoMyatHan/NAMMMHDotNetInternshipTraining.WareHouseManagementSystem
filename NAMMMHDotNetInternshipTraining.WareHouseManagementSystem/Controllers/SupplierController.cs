using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NAMMMHDotNetInternshipTraining.Database.AppDbModels;
using NAMMMHDotNetInternshipTraining.WareHouseManagementSystem.Models;

namespace NAMMMHDotNetInternshipTraining.WareHouseManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly AppDbContext db = new AppDbContext();

        [HttpGet]
        public IActionResult GetSupplier()
        {
            var lst = db.TblSuppliers.Include(x=>x.TblPurchases)
                .Select(x => new
                {
                    SupplierId = x.SupplierId,
                    SupplierName = x.SupplierName,
                    PhoneNo = x.Phone,
                    Address = x.Address,
                    IsDelete = x.IsDelete,

                    SaleItems = x.TblPurchases.SelectMany(s => s.TblPurchaseItems)
                .Select(si => new
                {
                    ProductName = si.Product.ProductName,
                    PurchaseDate = si.Purchase.PurchaseDate

                })
                .ToList()

                })

                .ToList();
            return Ok(lst);
        }

        [HttpGet("{id}")]
        public IActionResult GetSupplierById(int id)
        {
            var supplierData = db.TblSuppliers
         .Where(s => s.SupplierId == id && !s.IsDelete) // ဖျက်မထားတဲ့ Supplier ဖြစ်ရမယ်
         .Select(s => new
         {
             SupplierId = s.SupplierId,
             SupplierName = s.SupplierName,
             Phone = s.Phone,
             Address = s.Address,
             

             // 🔥 [အဓိကနေရာ] ဒီ Supplier ဆီကနေ ဝယ်ခဲ့သမျှ အဝယ်ဘေလ်အားလုံးထဲက ပစ္စည်းတွေကို စုစည်းထုတ်ယူခြင်း
             SuppliedProducts = s.TblPurchases
                 .SelectMany(p => p.TblPurchaseItems) // အဝယ်ဘေလ်အားလုံးထဲက Item တွေကို စုစည်းလိုက်တာပါ
                 .Select(pi => new
                 {
                    
                     ProductName = pi.Product.ProductName, // ပစ္စည်းနာမည်
                    
                     PurchaseDate = pi.Purchase.PurchaseDate // ဘယ်နေ့က ဝယ်ခဲ့လဲဆိုတဲ့ ရက်စွဲ
                 })
                 .ToList()
         })
         .FirstOrDefault();

            if (supplierData == null)
            {
                return NotFound("Supplier not found သို့မဟုတ် ဖျက်ပစ်ထားပြီးသားဖြစ်နေပါသည်။");
            }

            return Ok(supplierData);
        }

        [HttpPost]
        public IActionResult CreateSupplier(SupplierCreateRequestModel requestModel)
        {
            db.TblSuppliers.Add(new TblSupplier
            {
                SupplierName = requestModel.SupplierName,
                Phone = requestModel.Phone,
                Address = requestModel.Address,
                IsDelete = requestModel.IsDelete
            });

            var result = db.SaveChanges();

            return Ok(new SupplierCreateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Supplier created successfully" : "Fail to create Supplier"
            });
        }
        [HttpPut("{id}")]
        public IActionResult UpdateSupplier(int id, SupplierUpdateRequestModel requestModel)
        {
            var item = db.TblSuppliers.FirstOrDefault(x => x.SupplierId == id);
            if (item == null)
            {
                return NotFound(new SupplierUpdateResponseModel
                {
                    IsSuccess = false,
                    Message = "Supplier not found"
                });
            }

            // Database ထဲ ဒေတာထည့်သွင်းပြင်ဆင်ခြင်း
            item.SupplierName = requestModel.SupplierName;
            item.Phone = requestModel.Phone;
            item.Address = requestModel.Address;
            item.IsDelete = requestModel.IsDelete;

            var result = db.SaveChanges();

            return Ok(new SupplierUpdateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Supplier updated successfully" : "Fail to update Supplier"
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSupplier(int id)
        {
            var item = db.TblSuppliers.FirstOrDefault(x => x.SupplierId == id);
            if (item == null)
            {
                return NotFound(new SupplierUpdateResponseModel
                {
                    IsSuccess = false,
                    Message = "Can not Delete. Not found data"
                });
            }

            db.TblSuppliers.Remove(item);
            var result = db.SaveChanges();

            return Ok(new SupplierUpdateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Delete Successfully" : "Fail to Delete"
            });
        }

        [HttpPatch("{id}")]
        public IActionResult PatchSupplier(int id, SupplierUpdateRequestModel requestModel)
        {
            var item = db.TblSuppliers.FirstOrDefault(x => x.SupplierId == id);
            if (item == null)
            {
                return NotFound(new SupplierUpdateResponseModel
                {
                    IsSuccess = false,
                    Message = "Can not update. Data not found"
                });
            }

            int count = 0;
            if (!string.IsNullOrEmpty(requestModel.SupplierName))
            {
                count++;
                item.SupplierName = requestModel.SupplierName;
            }
            if (!string.IsNullOrEmpty(requestModel.Phone))
            {
                count++;
                item.Phone = requestModel.Phone;
            }
            if (!string.IsNullOrEmpty(requestModel.Address))
            {
                count++;
                item.Address = requestModel.Address;
            }

            // IsDelete က bool ဖြစ်လို့ လက်ခံရရှိမှု ရှိမရှိ စစ်ဆေးခြင်း
            count++;
            item.IsDelete = requestModel.IsDelete;

            if (count == 0)
            {
                return BadRequest(new SupplierUpdateResponseModel
                {
                    IsSuccess = false,
                    Message = "No fields to update"
                });
            }

            var result = db.SaveChanges();
            return Ok(new SupplierUpdateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Success" : "Not Success"
            });
        }
    }


}
