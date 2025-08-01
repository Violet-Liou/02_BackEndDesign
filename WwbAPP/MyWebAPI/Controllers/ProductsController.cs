using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
        //private readonly GoodStoreContext _context;

        //public ProductsController(GoodStoreContext context)
        //{
        //    _context = context;
        //}


        //4.7.8 修改ProductsController上方所注入的GoodStoreContext為GoodStoreContext2
        private readonly GoodStoreContextG2 _context;

        public ProductsController(GoodStoreContextG2 context)
        {
            _context = context;
        }

        // GET: api/Products
        //[HttpGet("productList")] //路由變成：/apiProducts/productList
        //[HttpGet("/productList")] //路由變成：/productList (/代表根目錄，根據路由設定，這裡的路由會覆蓋掉上面的路由設定)
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProduct(decimal price = 0)
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProduct(string? cateID, string? productName, decimal? minPrice, decimal? maxPrice, string? description, string? category)
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
            //var products = await _context.Product
            //                   .Include(p => p.Cate) // 同時載入Category資料
            //                   .Where(p => p.Price >= price)
            //                   .OrderBy(p => p.Price)
            //                   .Select(p => new ProductDTO{
            //                       ProductID = p.ProductID, // 指定欄位，賽入值
            //                       ProductName = p.ProductName,
            //                       Price = p.Price,
            //                       Description = p.Description,
            //                       Picture = p.Picture,
            //                       CateID = p.CateID,
            //                       CateName = p.Cate.CateName // 只選取需要的欄位，並包含Cate的CateName
            //                   })
            //                   .AsNoTracking() // 使用AsNoTracking()提高查詢效能 若無此行，可能會出現第一筆查到的資料被記錄，並覆蓋掉之後所查詢到的資料。
            //                   .ToListAsync();

            //4.4.1 將資料轉換的程式寫成函數並再次改寫Get Action(※這種寫法架構才會好※)
            //因原先兩個GET都有使用select相同欄位，把這個相同的部分取出做一個function，降低重複性，讓程式碼更乾淨易讀。
            //用DTO傳輸
            //var products = await _context.Product
            //                   .Include(p => p.Cate) // 同時載入Category資料
            //                   .Where(p => p.Price >= price)
            //                   .OrderBy(p => p.Price)
            //                   .Select(p => ItemProduct(p))
            //                   .AsNoTracking() // 使用AsNoTracking()提高查詢效能 若無此行，可能會出現第一筆查到的資料被記錄，並覆蓋掉之後所查詢到的資料。
            //                   .ToListAsync();

            //【關鍵字查詢功能】：建立多個where條件的寫法
            //var products =await _context.Product.Include(p => p.Cate) // 同時載入Category資料
            //                               .OrderBy(p => p.Price)
            //                               .Select(p => ItemProduct(p)).ToListAsync(); //先將資料轉成list物件
            //上面的寫法，會先將資料載入內存 >>傷效能(因為資料量大時，會造成記憶體不足)

            //4.4.8 修改先將資料載入內存的寫法
            //var products = _context.Product.Include(p => p.Cate) // 同時載入Category資料
            //                               .OrderBy(p => p.Price)
            //                               .Select(p => ItemProduct(p));

            var products = _context.Product.Include(p => p.Cate) // 同時載入Category資料
                                           .OrderBy(p => p.Price)
                                           .AsQueryable();

            //4.4.2 加入產品類別搜尋
            if (!string.IsNullOrEmpty(cateID))
            {
                /*products = products.Where(p => p.CateID == cateID).ToList();*/ //硬湊where語法，但不正確
                //4.4.8
                products = products.Where(p => p.CateID == cateID);
            }

            //4.4.3 加入產品名稱關鍵字搜尋
            if (!string.IsNullOrEmpty(productName))
            {
                //products = products.Where(p => p.ProductName.Contains(productName)).ToList();//硬湊where語法，但不正確
                //4.4.8
                products = products.Where(p => p.ProductName.Contains(productName));
            }

            //4.4.4 加入價格區間搜區
            if (minPrice.HasValue && maxPrice.HasValue)
            {
                //products = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();//硬湊where語法，但不正確
                //4.4.8
                products = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            }

            //4.4.5 加入產品敘述關鍵字搜尋
            if (!string.IsNullOrEmpty(description))
            {
                //products = products.Where(p => p.Description.Contains(description)).ToList();//硬湊where語法，但不正確
                //4.4.8
                products = products.Where(p => p.Description.Contains(description));
            }

            //自己做：加入產品類別名稱關鍵字搜尋
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Cate.CateName.Contains(description));
            }

            if (products == null || products.Count() == 0)
            {
                return NotFound("找不到產品資料");
            }


            //return products;
            //4.4.8
            //return await products.ToListAsync(); 
            //這樣執行後會發生錯誤，因為資料庫中沒有ProductDTO這個資料表，所以沒有辦法做where條件的篩選
            //
            return await products.Select(p => ItemProduct(p)).ToListAsync();
        }

        //4.6.1 新增一個Get Action GetProductFromSQL()並設定介接口為[HttpGet("fromSQL")]
        [HttpGet("fromSQL")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductFromSQL(string? cateID, string? productName, decimal? minPrice, decimal? maxPrice, string? description)
        {
            //4.6.2 用SQL語法撰寫與先前一樣的功能並使用DTO傳遞結果
            //var products = _context.Product.Include(c => c.Cate).OrderBy(p => p.Price).AsQueryable();

            string sql = "select p.ProductID, p.ProductName, p.Price, p.Description, p.Picture, p.CateID, c.CateName " +
                " from Product as p inner join Category as c on p.CateID=c.CateID where 1=1 ";

            List<SqlParameter> parameter = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(cateID))
            {

                sql += " and p.CateID = @cate ";
                parameter.Add(new SqlParameter("@cate", cateID));
            }

            if (!string.IsNullOrEmpty(productName))
            {

                sql += $" and p.ProductName like @productName ";
                parameter.Add(new SqlParameter("@productName", productName));
            }

            if (minPrice.HasValue && maxPrice.HasValue)
            {
                sql += $" and between @minPrice and @maxPrice ";
                parameter.Add(new SqlParameter("@minPrice", minPrice));
                parameter.Add(new SqlParameter("@maxPrice", maxPrice));
            }


            if (!string.IsNullOrEmpty(description))
            {
                sql += $" and p.Description like @description ";
                parameter.Add(new SqlParameter("@description", description));
            }

            //4.6.4 使用Swagger測試(這裡會發生錯誤，因為使用了合併查詢)
            //var products = await _context.Product.FromSqlRaw(sql).ToListAsync();

            //4.6.6 將_context.Product.FromSqlRaw(sql).ToListAsync();改為_context.ProductDTO.FromSqlRaw(sql).ToListAsync();
            var products = await _context.ProductDTO.FromSqlRaw(sql).ToListAsync();


            if (products == null || products.Count() == 0)
            {
                return NotFound("找不到產品資料");
            }

            return products;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(string id)
        {
            //var product = await _context.Product
            //                   .Include(p => p.Cate) // 同時載入Category資料
            //                   .Where(p => p.ProductID == id)
            //                   .OrderBy(p => p.Price)
            //                   .Select(p => new ProductDTO
            //                   {
            //                       ProductID = p.ProductID, // 指定欄位，賽入值
            //                       ProductName = p.ProductName,
            //                       Price = p.Price,
            //                       Description = p.Description,
            //                       Picture = p.Picture,
            //                       CateID = p.CateID,
            //                       CateName = p.Cate.CateName // 只選取需要的欄位，並包含Cate的CateName
            //                   })
            //                   .FirstOrDefaultAsync(); //只取到一筆資料

            //4.4.1 將資料轉換的程式寫成函數並再次改寫Get Action(※這種寫法架構才會好※)
            var product = await _context.Product
                               .Include(p => p.Cate) // 同時載入Category資料
                               .Where(p => p.ProductID == id)
                               .OrderBy(p => p.Price)
                               .Select(p => ItemProduct(p))
                               .FirstOrDefaultAsync(); //只取到一筆資料

            if (product == null)
            {
                return NotFound("找不到產品資料");
            }

            return product;
        }

        //4.8.2 在ProductsController中建立一個新的Get Action
        //4.8.3 設置介接口為[HttpGet("fromProc/{id}")]，Action名稱可自訂，並使用ProductDTO來傳遞資料
        [HttpGet("fromProc/{id}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductFromProc(string id)
        {
            //4.8.4 使用預存程序進行查詢(參數的傳遞請使用SqlParameter)
            string sql = $"exec getProductWithCateName @CateID";

            var cateID = new SqlParameter("@CateID", id);

            var products = await _context.ProductDTO.FromSqlRaw(sql, cateID).ToListAsync();
            //改成參數化

            if (products == null || products.Count() == 0)
            {
                return NotFound("找不到產品資料");
            }

            return products;
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

        [HttpPut("put2{id}")]
        public async Task<IActionResult> PutProd(string id, ProductPutDTO product)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var Pro = await _context.Product.FindAsync(id); //查找傳回來的資料使否有這個id

            if (Pro == null)
            {
                return NotFound("查無資料");
            }

            //檢查是否有新照片上傳
            if (product.Picture != null && product.Picture.Length != 0)
            {
                FileUpload(product.Picture, id);

            }

            Pro.ProductName = product.ProductName;
            Pro.Price= product.Price;
            Pro.Description = product.Description;

            _context.Entry(Pro).State = EntityState.Modified;

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

            return Ok(Pro);
        }

        //http通訊協定，只有兩種傳送資料的方式：POST(不從網址列取得資料)、GET(從網址列取得資料)
        //REST分有四種動作：GET(取得資料)、POST(新增資料)、PUT(更新資料)、DELETE(刪除資料)
        //除了GET以外，其他三種動作都會有資料傳送到伺服器端。
        //
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

        //5.2.4 建立一個新的Post Action，介接口設定為[HttpPost("PostWithPhoto")]，並加入上傳檔案的動作
        [HttpPost("PostWithPhoto")]
        public async Task<ActionResult<ProductPostDTO>> PostProductWithPhoto([FromForm] ProductPostDTO product)
        {
            //好的架構是，先將控制邏輯判斷寫在前面，後面再開始組需要上傳的東西(商業邏輯)

            //上傳檔案的處理
            if (product.Picture != null || product.Picture.Length == 0)
            {
                return BadRequest();
            }

            //5.2.5 將上傳檔案寫成一個獨立的方法

            ////判斷上傳的檔案是否為圖片格式
            //var extension = Path.GetExtension(product.Picture.FileName).ToLower();
            //var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" }; //允許的圖片副檔名
            ////想比於使用if去判斷每個副檔名，這樣寫法更簡潔、架構更好
            ////if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")

            //if(allowedExtensions.Contains(extension))
            //{
            //    //沒找到，將檔案踢回去 (因為使用的是ActionResult，所以一定是要使用HTTP的方法)
            //    return BadRequest("上傳的檔案不是有效的圖片格式。");
            //}

            ////檔案上傳的路徑
            //var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductPhotos");

            ////確保目錄存在
            //if (!Directory.Exists(uploadPath))
            //{
            //    Directory.CreateDirectory(uploadPath); //如果目錄不存在，則建立目錄
            //}

            ////如果有上傳檔案，則處理檔案上傳
            //var fileName = product.ProductID + Path.GetExtension(product.Picture.FileName); //使用ProductID作為檔名，並保留原始副檔名
            //var filePath = Path.Combine(uploadPath, fileName); //設定儲存路徑 "/wwwroot/ProductPhotos/XXXXX.jpg"

            ////將上傳的檔案儲存到指定路徑
            //using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            //    product.Picture.CopyToAsync(stream);
            //}

            string fileName = FileUpload(product.Picture, product.ProductID);

            if(fileName == "")
            {
                return BadRequest("上傳的檔案不是有效的圖片格式。");
            }

            Product p = new Product
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                Price = product.Price,
                Description = product.Description,
                Picture = fileName,
                CateID = product.CateID
            };


            //寫入資料庫
            _context.Product.Add(p);
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

        private static ProductDTO ItemProduct(Product p)
        {
            return new ProductDTO
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                Price = p.Price,
                Description = p.Description,
                Picture = p.Picture,
                CateID = p.CateID,
                CateName = p.Cate.CateName // 只選取需要的欄位，並包含Cate的CateName
            };
        }


        //5.2.5 將上傳檔案寫成一個獨立的方法
        private string FileUpload(IFormFile Photo, string PID)
        {
            //判斷上傳的檔案是否為圖片格式
            var extension = Path.GetExtension(Photo.FileName).ToLower();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" }; //允許的圖片副檔名
            //想比於使用if去判斷每個副檔名，這樣寫法更簡潔、架構更好
            //if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")

            if (! allowedExtensions.Contains(extension))
            {
                //沒找到，將檔案踢回去 (因為使用的是ActionResult，所以一定是要使用HTTP的方法)
                //return BadRequest("上傳的檔案不是有效的圖片格式。");
                //原先回傳 BadRequest，但因為這個方法是要回傳「string」，所以無法使用HTTP的方法，而是要回傳string類型的值
                return ""; //回傳空字串，表示上傳失敗
            }

            //檔案上傳的路徑
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductPhotos");

            //確保目錄存在
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath); //如果目錄不存在，則建立目錄
            }

            //如果有上傳檔案，則處理檔案上傳
            var fileName = PID + Path.GetExtension(Photo.FileName); //使用ProductID作為檔名，並保留原始副檔名
            var filePath = Path.Combine(uploadPath, fileName); //設定儲存路徑 "/wwwroot/ProductPhotos/XXXXX.jpg"

            //將上傳的檔案儲存到指定路徑
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                Photo.CopyToAsync(stream);
            }

            return fileName; //回傳檔名
        }

    }
}
