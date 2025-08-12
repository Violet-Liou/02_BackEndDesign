using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyWebAPI.DTOs;
using MyWebAPI.Models;
using MyWebAPI.QueryParameters;

namespace MyWebAPI.Services
{
    public class ProductService
    {
        private readonly GoodStoreContextG2 _context;

        public ProductService(GoodStoreContextG2 context)
        {
            _context = context;
        }


        //[HttpGet]
        //public async Task<List<ProductDTO>> GetProduct(string? cateID, string? productName, decimal? minPrice, decimal? maxPrice, string? description, string? category)
        public async Task<List<ProductDTO>> GetProduct(ProductParam productParam)
        {
            var products = _context.Product.Include(p => p.Cate) // 同時載入Category資料
                                           .OrderBy(p => p.Price)
                                           .AsQueryable();

            if (!string.IsNullOrEmpty(productParam.catID))
            {
                products = products.Where(p => p.CateID == productParam.catID);
            }

            if (!string.IsNullOrEmpty(productParam.productName))
            {
                products = products.Where(p => p.ProductName.Contains(productParam.productName));
            }

            if (productParam.minPrice.HasValue && productParam.maxPrice.HasValue)
            {
                products = products.Where(p => p.Price >= productParam.minPrice && p.Price <= productParam.maxPrice);
            }

            if (!string.IsNullOrEmpty(productParam.description))
            {
                products = products.Where(p => p.Description.Contains(productParam.description));
            }

            var productList = await products.Select(p => ItemProduct(p)).ToListAsync();

            return productList;
        }

        
        //[HttpGet("{id}")]
        public async Task<ProductDTO> GetProduct(string id)
        {
            var product = await _context.Product
                               .Include(p => p.Cate) // 同時載入Category資料
                               .Where(p => p.ProductID == id)
                               .OrderBy(p => p.Price)
                               .Select(p => ItemProduct(p))
                               .FirstOrDefaultAsync(); //只取到一筆資料

            //if (product == null)
            //{
            //    return NotFound("找不到產品資料");
            //}

            return product;
        }

        //[HttpGet("fromSQL")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductFromSQL(ProductParam productParam)
        {

            string sql = "select p.ProductID, p.ProductName, p.Price, p.Description, p.Picture, p.CateID, c.CateName " +
                " from Product as p inner join Category as c on p.CateID=c.CateID where 1=1 ";

            List<SqlParameter> parameter = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(productParam.catID))
            {

                sql += " and p.CateID = @cate ";
                parameter.Add(new SqlParameter("@cate", productParam.catID));
            }

            if (!string.IsNullOrEmpty(productParam.productName))
            {

                sql += $" and p.ProductName like @productName ";
                parameter.Add(new SqlParameter("@productName", $"%{productParam.productName}%"));
            }

            if (productParam.minPrice.HasValue && productParam.maxPrice.HasValue)
            {
                sql += $" and between @minPrice and @maxPrice ";
                parameter.Add(new SqlParameter("@minPrice", productParam.minPrice));
                parameter.Add(new SqlParameter("@maxPrice", productParam.maxPrice));
            }


            if (!string.IsNullOrEmpty(productParam.description))
            {
                sql += $" and p.Description like @description ";
                parameter.Add(new SqlParameter("@description", $"%{productParam.description}%"));
            }

            var products = await _context.ProductDTO.FromSqlRaw(sql).ToListAsync();

            return products;
        }

        //[HttpGet("fromProc/{id}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductFromProc(string id)
        {
            //4.8.4 使用預存程序進行查詢(參數的傳遞請使用SqlParameter)
            string sql = $"exec getProductWithCateName @CateID";

            var cateID = new SqlParameter("@CateID", id);

            var products = await _context.ProductDTO.FromSqlRaw(sql, cateID).ToListAsync();
            //改成參數化

            //if (products == null || products.Count() == 0)
            //{
            //    return NotFound("找不到產品資料");
            //}

            return products;
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
    }
}
