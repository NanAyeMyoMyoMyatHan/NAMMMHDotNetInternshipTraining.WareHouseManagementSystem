using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var lst = db.TblSuppliers.ToList();
            return Ok(lst);
        }

        [HttpGet("{id}")]
        public IActionResult GetSupplierById(int id)
        {
            var item = db.TblSuppliers.FirstOrDefault(x => x.SupplierId == id);
            if (item == null)
            {
                return NotFound("There is no Data");
            }
            return Ok(item);
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
