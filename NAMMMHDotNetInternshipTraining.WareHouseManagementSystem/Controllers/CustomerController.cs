using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NAMMMHDotNetInternshipTraining.Database.AppDbModels;
using NAMMMHDotNetInternshipTraining.WareHouseManagementSystem.Models;

namespace NAMMMHDotNetInternshipTraining.WareHouseManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext db = new AppDbContext();

        [HttpGet]
        public IActionResult GetCustomer()
        {
            var lst = db.TblCustomers.ToList();
            return Ok(lst);
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomerById(int id)
        {
            var item = db.TblCustomers.FirstOrDefault(x => x.CustomerId == id);
            if (item == null)
            {
                return NotFound("There is no Data");
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateCustomer(CustomerCreateRequestModel requestModel)
        {
            db.TblCustomers.Add(new TblCustomer
            {
                CustomerName = requestModel.CustomerName,
                Phone = requestModel.Phone,
                Address = requestModel.Address,
                IsDelete = requestModel.IsDelete,
                CreatedDateTime = DateTime.Now
            });

            var result = db.SaveChanges();

            return Ok(new CustomerCreateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Customer created successfully" : "Fail to create Customer"
            });
        }
        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, CustomerUpdateRequestModel requestModel)
        {
            var item = db.TblCustomers.FirstOrDefault(x => x.CustomerId == id);
            if (item == null)
            {
                return NotFound(new CustomerUpdateResponseModel
                {
                    IsSuccess = false,
                    Message = "Customer not found"
                });
            }

            item.CustomerName = requestModel.CustomerName;
            item.Phone = requestModel.Phone;
            item.Address = requestModel.Address;
            item.IsDelete = requestModel.IsDelete;

            var result = db.SaveChanges();

            return Ok(new CustomerUpdateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Customer updated successfully" : "Fail to update Customer"
            });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            var item = db.TblCustomers.FirstOrDefault(x => x.CustomerId == id);
            if (item == null)
            {
                return NotFound(new CustomerUpdateResponseModel
                {
                    IsSuccess = false,
                    Message = "Can not Delete. Not found data"
                });
            }

            db.TblCustomers.Remove(item);
            var result = db.SaveChanges();

            return Ok(new CustomerUpdateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Delete Successfully" : "Fail to Delete"
            });
        }

        [HttpPatch("{id}")]
        public IActionResult PatchCustomer(int id, CustomerUpdateRequestModel requestModel)
        {
            var item = db.TblCustomers.FirstOrDefault(x => x.CustomerId == id);
            if (item == null)
            {
                return NotFound(new CustomerUpdateResponseModel
                {
                    IsSuccess = false,
                    Message = "Can not update. Data not found"
                });
            }

            int count = 0;
            if (!string.IsNullOrEmpty(requestModel.CustomerName))
            {
                count++;
                item.CustomerName = requestModel.CustomerName;
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

            count++;
            item.IsDelete = requestModel.IsDelete;

            if (count == 0)
            {
                return BadRequest(new CustomerUpdateResponseModel
                {
                    IsSuccess = false,
                    Message = "No fields to update"
                });
            }

            var result = db.SaveChanges();
            return Ok(new CustomerUpdateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Success" : "Not Success"
            });
        }
    }
}
