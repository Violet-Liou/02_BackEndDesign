using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.DTOs;
using MyWebAPI.Models;
using MyWebAPI.Services;

namespace MyWebAPI.Controllers
{
    //3.1.4 修改API介接路由為「api[controller]」
    [Route("api[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly GoodStoreContext _context;

        //8.2.4 在CategoriesController裡注入CategoryService服務
        //★注意：注入CategoryService前，必須讓該Service已經在program.cs中註冊過
        private readonly CategoryService _categoryService;


        public CategoriesController(GoodStoreContext context, CategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        //8.2.5 改寫CategoriesController裡的兩個Get Action寫法，僅留下控制邏輯
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategory()
        {
            //var result = await _context.Category
            //    .Include(c => c.Product) //等同SQL上的 JOIN
            //    .Select(c => ItemCategory(c))
            //    .AsNoTracking()
            //    .ToListAsync();

            var categories = await _categoryService.GetCategory();

            //控制邏輯：如果沒有找到任何資料，則回傳NotFound
            if (categories == null || !categories.Any())
            {
                return NotFound("沒有找到任何資料");
            }

            return categories;
        }


        //8.2.5 改寫CategoriesController裡的兩個Get Action寫法，僅留下控制邏輯
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(string id)
        {
            //var category = await _context.Category
            //                    .Include(c => c.Product)
            //                    .Where(c => c.CateID == id)
            //                    .Select(c => ItemCategory(c))
            //                    .FirstOrDefaultAsync();

            var category = await _categoryService.GetCategory(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }


        //8.2.7 將Post、Put及Delete重構
        [HttpPut("put2{id}")]
        public async Task<IActionResult> PutCate(string id,[FromForm] CategoryPutDTO category)
        {
            //控制邏輯 >> if....
            if (id == null)
            {
                return BadRequest();
            }

            //var cate = await _context.Category.FindAsync(id); //查找傳回來的資料使否有這個id
            //Find >> 代表有要從資料庫比對資料，這部分應是屬於商業邏輯的部分，所以拉出來在Service裡面寫 (Task<Category> getCategory(string id))
            //修改成
            var cateOld = await _categoryService.getCategory(id);

            if (cateOld == null)
            {
                return NotFound("查無資料");
            }

            //cate.CateName = category.CateName; //只修改CateName

            //_context.Entry(cate).State = EntityState.Modified;
            var cate = await _categoryService.UpdateCategory(cateOld, category);

            //try
            //{
            //    await _context.SaveChangesAsync(); //真正寫入資料庫
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!CategoryExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return Ok(cate); //讓OK回傳整個cate的內容(等同update以後，直接把資料select出來)
        }

        [HttpPost]
        public async Task<ActionResult<CategoryPostDTO>> PostCategory(CategoryPostDTO category)
        {
            //要把DTO轉成原先的物件類別
            //Category cata = new Category()
            //{
            //    CateID = category.CateID,
            //    CateName = category.CateName
            //};

            //_context.Category.Add(cata);
            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateException)
            //{
            //    if (CategoryExists(category.CateID))
            //    {
            //        return Conflict();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return CreatedAtAction("GetCategory", new { id = category.CateID }, category);

            if (CategoryExists(category.CateID))
            {
                return BadRequest();
            }

            var cate = _categoryService.InsertCategory(category);

            return Ok(cate);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            //var category = await _context.Category.FindAsync(id);
            var category = await _categoryService.getCategory(id); //商業邏輯部分已被拉出來重作

            if (category == null)
            {
                return NotFound();
            }

            //_context.Category.Remove(category);
            //await _context.SaveChangesAsync();

            var result = await _categoryService.DeleteCategory(category);

            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        //Scaffold的時候，系統就已經自動寫好
        //確認是否存在這個Category的ID
        private bool CategoryExists(string id)
        {
            return _context.Category.Any(e => e.CateID == id);
        }

        //這個區塊已經拉到CategoryService裡面了
        ////4.5.2 建立CategoryDTO類別
        //private static CategoryDTO ItemCategory(Category c)
        //{
        //    var result =  new CategoryDTO
        //    {
        //        CateID = c.CateID,
        //        CateName = c.CateName,
        //        Product = c.Product
        //    };

        //    return result;
        //}
    }
}
