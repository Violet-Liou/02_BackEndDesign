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
using MyWebAPI.QueryParameters;
using MyWebAPI.Services;

namespace MyWebAPI.Controllers
{
    //3.2.4 修改API介接路由為「api[controller]」
    [Route("api[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly GoodStoreContextG2 _context;
        //8.3.4 在ProductsController裡注入ProductService服務
        private readonly ProductService _productService;

        public ProductsController(GoodStoreContextG2 context, ProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        //8.3.5 改寫ProductsController裡的Get Action寫法，僅留下控制邏輯
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProduct(string? cateID, string? productName, decimal? minPrice, decimal? maxPrice, string? description, string? category)
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProduct(ProductParam productParam)
        {
            //商業邏輯：從資料庫中取得產品資料，並根據查詢參數進行篩選和排序
            //var products = _context.Product.Include(p => p.Cate) // 同時載入Category資料
            //                               .OrderBy(p => p.Price)
            //                               .AsQueryable();

            //if (!string.IsNullOrEmpty(cateID))
            //{
            //    products = products.Where(p => p.CateID == cateID);
            //}

            //if (!string.IsNullOrEmpty(productName))
            //{
            //    products = products.Where(p => p.ProductName.Contains(productName));
            //}

            //if (minPrice.HasValue && maxPrice.HasValue)
            //{
            //    products = products.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            //}

            //if (!string.IsNullOrEmpty(description))
            //{
            //    products = products.Where(p => p.Description.Contains(description));
            //}

            //if (!string.IsNullOrEmpty(category))
            //{
            //    products = products.Where(p => p.Cate.CateName.Contains(description));
            //}

            var products = await _productService.GetProduct(productParam);

            if (products == null || products.Count() == 0)
            {
                return NotFound("找不到產品資料");
            }

            //return await products.Select(p => ItemProduct(p)).ToListAsync();
            return products;
        }

        [HttpGet("fromSQL")]
        //public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductFromSQL(string? cateID, string? productName, decimal? minPrice, decimal? maxPrice, string? description)
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductFromSQL([FromQuery] ProductParam productParam)
        //[FromQuery]  顯著標示資料傳輸的方式
        {

            //string sql = "select p.ProductID, p.ProductName, p.Price, p.Description, p.Picture, p.CateID, c.CateName " +
            //    " from Product as p inner join Category as c on p.CateID=c.CateID where 1=1 ";

            //List<SqlParameter> parameter = new List<SqlParameter>();

            //if (!string.IsNullOrEmpty(cateID))
            //{

            //    sql += " and p.CateID = @cate ";
            //    parameter.Add(new SqlParameter("@cate", cateID));
            //}

            //if (!string.IsNullOrEmpty(productName))
            //{

            //    sql += $" and p.ProductName like @productName ";
            //    parameter.Add(new SqlParameter("@productName", productName));
            //}

            //if (minPrice.HasValue && maxPrice.HasValue)
            //{
            //    sql += $" and between @minPrice and @maxPrice ";
            //    parameter.Add(new SqlParameter("@minPrice", minPrice));
            //    parameter.Add(new SqlParameter("@maxPrice", maxPrice));
            //}


            //if (!string.IsNullOrEmpty(description))
            //{
            //    sql += $" and p.Description like @description ";
            //    parameter.Add(new SqlParameter("@description", description));
            //}

            //var products = await _context.ProductDTO.FromSqlRaw(sql).ToListAsync();

            var products = await _productService.GetProductFromSQL(productParam);


            if (products == null) //|| products.Count() == 0 這句一直出現錯誤
            {
                return NotFound("找不到產品資料");
            }

            return products;
        }

        //8.3.5 改寫ProductsController裡的Get Action寫法，僅留下控制邏輯
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(string id)
        {
            //商業邏輯
            //var product = await _context.Product
            //                   .Include(p => p.Cate) // 同時載入Category資料
            //                   .Where(p => p.ProductID == id)
            //                   .OrderBy(p => p.Price)
            //                   .Select(p => ItemProduct(p))
            //                   .FirstOrDefaultAsync(); //只取到一筆資料

            var product = await _productService.GetProduct(id);

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
            //string sql = $"exec getProductWithCateName @CateID";

            //var cateID = new SqlParameter("@CateID", id);

            //var products = await _context.ProductDTO.FromSqlRaw(sql, cateID).ToListAsync();
            //改成參數化

            var products = await _productService.GetProductFromProc(id);

            if (products == null) //|| products.Count() == 0
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

            string fileName = await FileUpload(product.Picture, product.ProductID);

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

        //7.1.1 改寫ProductsController中Delete Action內容，加入刪除照片的功能
        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            //await 是當你需要等待某個非同步方法完成時使用。(對應async方法)
            //刪除照片
            if (! await FileDelete(product.Picture))
            {
                return BadRequest("刪除商品照片失敗，請檢查檔案是否存在或權限問題。");
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("ByCateID")]
        public async Task<IActionResult> DeleteProductByCateID(string cateID)
        {
            var products = await _context.Product.Where(p => p.CateID == cateID).ToListAsync();
            if (products == null)
            {
                return NotFound();
            }
            
            
            foreach(var p in products) 
            {
                //刪除照片
                if (!await FileDelete(p.Picture))
                {
                    return BadRequest("刪除商品照片失敗，請檢查檔案是否存在或權限問題。");
                }
                //刪除商品資料
                _context.Product.Remove(p);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                return NotFound("刪除商品失敗，請檢查商品是否存在。");
            }

            return NoContent();
        }

        private bool ProductExists(string id)
        {
            return _context.Product.Any(e => e.ProductID == id);
        }

        //4.4.1 將資料轉換的程式寫成函數並再次改寫Get Action(※這種寫法架構才會好※)
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
        private async Task<string> FileUpload(IFormFile Photo, string PID)
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

        //7.1.2 將刪除照片功能另建立FileDelete()方法
        private async Task<bool> FileDelete(string fileName)
        {
            //檔案上傳的路徑
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductPhotos");
            var filePath = Path.Combine(path, fileName);


            //防止發生例外，先做檢查
            if (!System.IO.File.Exists(filePath))
            {
                try
                {
                    System.IO.File.Delete(filePath); //刪除檔案

                    return true; //檔案刪除成功
                }
                catch (Exception ex)
                {
                    //處理例外情況
                    Console.WriteLine($"刪除檔案時發生錯誤: {ex.Message}");

                    return false; //檔案刪除失敗
                }
            }
            return false; //檔案不存在，無法刪除
        }

    }
}
