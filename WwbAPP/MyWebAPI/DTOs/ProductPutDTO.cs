using System.ComponentModel.DataAnnotations;
using static MyWebAPI.ValidationAttributes.MyValidator;

namespace MyWebAPI.DTOs
{
    public class ProductPutDTO
    {
        //ProductID是主鍵值，不應該可以被修改

        [Required]
        [StringLength(40)]
        [ProductNameCheck]
        public string ProductName { get; set; } = null!;

        [Required]
        [Range(0, 1000000)]
        public decimal Price { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        public IFormFile? Picture { get; set; } = null!;

        //[Required]
        //[RegularExpression("[A-Z][1-9]")]
        //public string CateID { get; set; } = null!;
        //CateID不應該可以被修改，可能會造成以前的資料也被連動了，所以如果有需要確保過往資料的正確性，就不能修改
    }
}
