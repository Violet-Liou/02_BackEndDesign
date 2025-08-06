using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.DTOs;
using MyWebAPI.Models;

namespace MyWebAPI.Controllers
{
    //3.1.4 修改API介接路由為「api[controller]」
    [Route("api[controller]")]
    [ApiController]
    public class CategoriesOLDController : ControllerBase
    {
        private readonly GoodStoreContext _context;

        public CategoriesOLDController(GoodStoreContext context)
        {
            _context = context;
        }

        //4.5.3 改寫CategoriesController裡的第一個Get Action
        // GET: api/Categories
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategory()
        {
            //var result = await _context.Category.ToListAsync();

            //for (var i = 0; i < result.Count(); i++)
            //{
            //    result[i].Product = await _context.Product
            //                        .Where(p => p.CateID == result[i].CateID)
            //                        .AsNoTracking().ToListAsync();
            //}

            var result = await _context.Category
                .Include(c => c.Product) //等同SQL上的 JOIN
                .Select(c => ItemCategory(c))
                .AsNoTracking()
                .ToListAsync();

            return result;
        }

        //4.5.6 改寫CategoriesController裡的第二個Get Action
        // GET: api/Categories/5
        [HttpGet("{id}")]
        //public async Task<ActionResult<Category>> GetCategory(string id)
        public async Task<ActionResult<CategoryDTO>> GetCategory(string id)
        {
            //var category = await _context.Category.FindAsync(id);
            //4.5.7 使用Include()同時取得關聯資料並以CategoryDTO傳遞
            var category = await _context.Category
                                .Include(c => c.Product)
                                .Where(c => c.CateID == id)
                                .Select(c => ItemCategory(c))
                                .FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")] //系統自己產生的
        public async Task<IActionResult> PutCategory(string id, Category category)
        {
            if (id != category.CateID)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPut("put2{id}")] //老師教的
        public async Task<IActionResult> PutCate(string id,[FromForm] CategoryPutDTO category)
        {
            //if (id != category.CateID)
            //{
            //    return BadRequest();
            //}

            //控制邏輯 >> if....
            if (id == null)
            {
                return BadRequest();
            }

            var cate = await _context.Category.FindAsync(id); //查找傳回來的資料使否有這個id

            if(cate == null)
            {
                return NotFound("查無資料");
            }

            cate.CateName = category.CateName; //只修改CateName

            _context.Entry(cate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync(); //真正寫入資料庫
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //return NoContent();
            return Ok(cate); //讓OK回傳整個cate的內容(等同update以後，直接把資料select出來)
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //5.3.10 修改CategoriesController的Post方法，使其傳遞CategoryPostDTO
        [HttpPost]
        public async Task<ActionResult<CategoryPostDTO>> PostCategory(CategoryPostDTO category)
        {
            //5.3.11 修改Post Action 內的寫法

            //要把DTO轉成原先的物件類別
            Category cata = new Category()
            {
                CateID = category.CateID,
                CateName = category.CateName
            };

            _context.Category.Add(cata);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CategoryExists(category.CateID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCategory", new { id = category.CateID }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Category.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(string id)
        {
            return _context.Category.Any(e => e.CateID == id);
        }

        //4.5.2 建立CategoryDTO類別
        private static CategoryDTO ItemCategory(Category c)
        {
            var result =  new CategoryDTO
            {
                CateID = c.CateID,
                CateName = c.CateName,
                Product = c.Product
            };

            return result;
        }
    }
}
