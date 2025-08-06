using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.DTOs;
using MyWebAPI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyWebAPI.Services
{
    public class CategoryService
    {
        private readonly GoodStoreContext _context;

        public CategoryService(GoodStoreContext context)
        {
            _context = context;
        }

        //8.2.1 在Service資料夾中建立CategoryService，並將CategoriesController裡的兩個Get Action相關的商業邏輯移至此撰寫
        //      (包括ItemProduct()方法一併移入CategoryService)

        //第一步：從CategoriesController複製get
        //第二步：刪除[HTTP]相關  >>因為這是Service，不是Controller，所以不需要
        //第三步：刪除控制邏輯部分 >>if...else...的這種
        //商業邏輯：資料具體要取得那些的方法
        //第四步：修改資料回傳類型  >>沒有ActionResult，因為不是HTTP
        //第五步：補上紅線部分缺失的function

        //[HttpGet]
        public async Task<List<CategoryDTO>> GetCategory()
        {

            var result = await _context.Category
                .Include(c => c.Product) //等同SQL上的 JOIN
                .Select(c => ItemCategory(c))
                .AsNoTracking()
                .ToListAsync();

            return result;
        }

        //[HttpGet("{id}")]
        public async Task<CategoryDTO> GetCategory(string id)
        {
            var category = await _context.Category
                                .Include(c => c.Product)
                                .Where(c => c.CateID == id)
                                .Select(c => ItemCategory(c))
                                .FirstOrDefaultAsync();

            //以下為控制邏輯
            //if (category == null)
            //{
            //    return NotFound();
            //}

            return category;
        }

        private static CategoryDTO ItemCategory(Category c)
        {
            var result = new CategoryDTO
            {
                CateID = c.CateID,
                CateName = c.CateName,
                Product = c.Product
            };

            return result;
        }

        //將在Controller裡的FindAsync(id)部分，拉到這邊寫 (應是屬於商業邏輯的部分)
        public async Task<Category> getCategory(string id)
        {
            return await _context.Category.FindAsync(id);
        }

        [HttpPut("put2{id}")]
        public async Task<Category> UpdateCategory(Category cate, [FromForm] CategoryPutDTO category)
        {
            cate.CateName = category.CateName; //只修改CateName

            _context.Entry(cate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync(); //真正寫入資料庫
            }
            catch (DbUpdateConcurrencyException)
            {
                //    if (!CategoryExists(id))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                throw;
                //}
            }

            //return NoContent();
            //return Ok(cate); //讓OK回傳整個cate的內容(等同update以後，直接把資料select出來)
            return cate; //直接回傳更新後的cate物件
        }

        //[HttpPost]
        public async Task<Category> InsertCategory(CategoryPostDTO category)
        {
            //要把DTO轉成原先的物件類別
            Category cate = new Category()
            {
                CateID = category.CateID,
                CateName = category.CateName
            };

            _context.Category.Add(cate);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                //if (CategoryExists(category.CateID))
                //{
                //    return Conflict();
                //}
                //else
                //{
                throw;
                //}
            }

            //return CreatedAtAction("GetCategory", new { id = category.CateID }, category);
            return cate;
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCategory(string id)
        public async Task<bool> DeleteCategory(Category cate)
        {
            //var category = await _context.Category.FindAsync(id);
            //var category = await _categoryService.getCategory(id); //商業邏輯部分已被拉出來重作

            //if (category == null)
            //{
            //    return NotFound();
            //}

            _context.Category.Remove(cate);
            //await _context.SaveChangesAsync();

            //return NoContent();

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                //如果刪除失敗，可能是因為有關聯的資料存在
                return false; //刪除失敗
            }
            return true; 
        }
    }
}
