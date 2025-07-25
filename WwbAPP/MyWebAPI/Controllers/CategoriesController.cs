﻿using System;
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
    public class CategoriesController : ControllerBase
    {
        private readonly GoodStoreContext _context;

        public CategoriesController(GoodStoreContext context)
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
                .Include(c => c.Product)
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
        [HttpPut("{id}")]
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

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory([FromForm]Category category)
        {
            _context.Category.Add(category);
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
