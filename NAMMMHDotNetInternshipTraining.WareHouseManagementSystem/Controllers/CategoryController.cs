using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NAMMMHDotNetInternshipTraining.Database.AppDbModels;
using NAMMMHDotNetInternshipTraining.WareHouseManagementSystem.Models;

namespace NAMMMHDotNetInternshipTraining.WareHouseManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext db = new AppDbContext();
        [HttpGet]
        public IActionResult GetCategory()
        {
            var lst = db.TblCategories.Include(x=>x.TblProducts)
                .Select(x => new
                {
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName,
                    IsDelete = x.IsDelete,
                    CreatedDateTime = x.CreatedDateTime,

                    // 👇 ဒီနေရာမှာ မင်းဖြစ်ချင်တဲ့အတိုင်း Product Name သီးသန့်ပဲ List လုပ်ပြီး ထုတ်ပေးလိုက်တာပါ
                    TblProducts = x.TblProducts.Select(p => p.ProductName).ToList()
                })
                .ToList();
            return Ok(lst);
        }
        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var item = db.TblCategories.FirstOrDefault(x=>x.CategoryId==id);
            if(item == null)
            {
                return NotFound("There is no Data");
            }
            return Ok(item);
        }
        [HttpPost]
        public IActionResult CreateCategory(CategoryCreateRequestModel requestModel)
        {
            db.TblCategories.Add(new TblCategory
            {
                CategoryName = requestModel.CategoryName,
                CreatedDateTime = DateTime.Now,
                IsDelete = requestModel.IsDelete
            });
            var result = db.SaveChanges();
            return Ok(new CategoryCreateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Category created successfully" : "Fail to create Category"
            });

        }
        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id,CategoryUpdateRequestModel requestModel)
        {
            var item = db.TblCategories.FirstOrDefault(x=> x.CategoryId==id);
            if( item == null)
            {
                return NotFound(new CategoriesUpdateResponseModel
                {
                    IsSuccess = false,
                    Message = "Category not found"
                });
            }
            item.CategoryName = requestModel.CategoryName;
            item.CreatedDateTime = requestModel.CreatedDateTime;
            item.IsDelete = requestModel.IsDelete;
            var result = db.SaveChanges();
            
                return Ok(new CategoriesUpdateResponseModel
                {
                    IsSuccess = result > 0,
                    Message = result>0?"Category update successfully":"Fail to update"

                });
            
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var item = db.TblCategories.FirstOrDefault( x=> x.CategoryId==id);
            if ( item == null )
            {
                return NotFound(new CategoriesUpdateResponseModel
                {
                    IsSuccess = false,
                    Message = "Can not Delete .Not found data"
                });
            }
            db.TblCategories.Remove(item);
            var result = db.SaveChanges();
            return Ok(new CategoriesUpdateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Delete Successfully": "Fail to Delete"
            });
        }
        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id ,CategoryUpdateRequestModel requestModel)
        {
            var item = db.TblCategories.FirstOrDefault(x => x.CategoryId == id);
            if ( item == null )
            {
                return NotFound(new CategoriesUpdateResponseModel
                {
                    IsSuccess = false,
                    Message = "Can not update .Data not found"
                });
            }
            item.CategoryName = requestModel.CategoryName;
            item.CreatedDateTime = requestModel.CreatedDateTime;
            item.IsDelete = requestModel.IsDelete;
            var result = db.SaveChanges();
            return Ok(new CategoriesUpdateResponseModel
            {
                IsSuccess = result > 0,
                Message = result > 0 ? "Update Success" : "Failed to Update"
            });
            int count = 0;
            if (!string.IsNullOrEmpty(requestModel.CategoryName))
            {
                count++;
                item.CategoryName= requestModel.CategoryName;
            }
            if (!string.IsNullOrEmpty(requestModel.CreatedDateTime.ToString()))
            {
                count++;
                item.CreatedDateTime= requestModel.CreatedDateTime;
            }
            if (!string.IsNullOrEmpty(requestModel.IsDelete.ToString()))
            {
                count++;
                item.IsDelete= requestModel.IsDelete;
            }
            if(count == 0)
            {
                return NotFound(new CategoriesUpdateResponseModel
                {
                    IsSuccess = false,
                    Message = "No fiels to update"
                });
                var results = db.SaveChanges();
                return Ok(new CategoriesUpdateResponseModel
                {
                    IsSuccess = results > 0,
                    Message = result > 0 ? "Success" : "Not Success"
                });

            }

        }
        
    }
}
