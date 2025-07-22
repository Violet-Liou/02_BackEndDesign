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
    //3.2.4 修改API介接路由為「api[controller]」
    [Route("api[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly GoodStoreContext _context;

        public ProductsController(GoodStoreContext context)
        {
            _context = context;
        }

        // GET: api/Products
        //[HttpGet("productList")] //路由變成：/apiProducts/productList
        //[HttpGet("/productList")] //路由變成：/productList (/代表根目錄，根據路由設定，這裡的路由會覆蓋掉上面的路由設定)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProduct(decimal price = 0)
        {
            //4.1.2 使用Include()同時取得關聯資料
            //4.1.3 使用Where()改變查詢的條件並測試
            //4.1.4 使用OrderBy()相關排序方法改變資料排序並測試
            //var products = await _context.Product.Include(c => c.Cate)
            //                                     .Where(p => p.Price >= price)
            //                                     .OrderBy(p => p.Price)
            //                                     .AsNoTracking() // 使用AsNoTracking()提高查詢效能
            //                                     .ToListAsync();

            //4.1.5 使用Select()抓取特定的欄位並測試(這樣做無法return)
            //var products = await _context.Product
            //                    .Include(p => p.Cate) // 同時載入Category資料
            //                    .Where(p => p.Price >= price)
            //                    .OrderBy(p => p.Price)
            //                    .Select(p => new {
            //                       p.ProductID,
            //                       p.ProductName,
            //                       p.Price,
            //                       p.Description,
            //                       p.CateID,
            //                       p.Cate.CateName // 只選取需要的欄位，並包含Cate的CateName
            //                    })
            //                    .AsNoTracking() // 使用AsNoTracking()提高查詢效能
            //                    .ToListAsync();

            //return products; //這裡返回匿名類型的列表，無法直接返回Product實體(product裡沒有CateName欄位)

            //4.2.3 改寫ProductsController裡的Get Action
            var products = await _context.Product
                               .Include(p => p.Cate) // 同時載入Category資料
                               .Where(p => p.Price >= price)
                               .OrderBy(p => p.Price)
                               .Select(p => new ProductDTO{
                                   ProductID = p.ProductID, // 指定欄位，賽入值
                                   ProductName = p.ProductName,
                                   Price = p.Price,
                                   Description = p.Description,
                                   Picture = p.Picture,
                                   CateID = p.CateID,
                                   CateName = p.Cate.CateName // 只選取需要的欄位，並包含Cate的CateName
                               })
                               .AsNoTracking() // 使用AsNoTracking()提高查詢效能 若無此行，可能會出現第一筆查到的資料被記錄，並覆蓋掉之後所查詢到的資料。
                               .ToListAsync();

            return products;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(string id)
        {
            var product = await _context.Product
                               .Include(p => p.Cate) // 同時載入Category資料
                               .Where(p => p.ProductID == id)
                               .OrderBy(p => p.Price)
                               .Select(p => new ProductDTO
                               {
                                   ProductID = p.ProductID, // 指定欄位，賽入值
                                   ProductName = p.ProductName,
                                   Price = p.Price,
                                   Description = p.Description,
                                   Picture = p.Picture,
                                   CateID = p.CateID,
                                   CateName = p.Cate.CateName // 只選取需要的欄位，並包含Cate的CateName
                               })
                               .FirstOrDefaultAsync(); //只取到一筆資料

            if (product == null)
            {
                return NotFound("找不到產品資料");
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(string id, Product product)
        {
            if (id != product.ProductID)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Product.Add(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductExists(product.ProductID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProduct", new { id = product.ProductID }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(string id)
        {
            return _context.Product.Any(e => e.ProductID == id);
        }
    }
}
