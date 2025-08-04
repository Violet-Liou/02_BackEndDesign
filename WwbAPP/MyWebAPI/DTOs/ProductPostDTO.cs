using MyWebAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MyWebAPI.ValidationAttributes.MyValidator;

namespace MyWebAPI.DTOs
{
    //5.2.3 建立一個ProductPostDTO給Post利用DTO傳遞資料
    public class ProductPostDTO
    {
        //5.3.1 在ProductPostDTO.cs加入需要的內建驗證器(Validator)
        [Required]
        [RegularExpression("[A-Z][1-9][0-9]{3}")]
        public string ProductID { get; set; } = null!;

        [Required]
        [StringLength(40)]
        //[ProductNameCheck] //自訂驗證器
        [ProductNameCheck(ErrorMessage ="請打三個字")] //這邊的ErrorMessage會覆蓋掉自訂驗證器所設定的錯誤訊息 
        public string ProductName { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        [Required]
        public IFormFile Picture { get; set; } = null!;

        [Required]
        [RegularExpression("[A-Z][1-9]")]
        public string CateID { get; set; } = null!;
    }

    //商業邏輯：屬於自己的特殊邏輯(商業機密)、演算法，與世上沒有共用
    //MVC的架構中，商業邏輯要寫在「Model」，控制邏輯寫在「Controller」
    //前端表單的部分，只能使用「內建的驗證」，若是有設定自訂驗證器的部分，就一定要POST出去，才會開始驗證
    //類別名稱絕對不能重複!!!!

    //5.3.3 在ProductPostDTO.cs加入自訂驗證器(使用ValidationAttribute物件)
    //public class ProductNameCheck: ValidationAttribute //因為是驗證器的class，所以需要繼承「ValidationAttribute」
    //{
    //    //ValidationResult >> 驗證結果
    //    //ActionResult >> 動作結果
    //    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    //    {
    //        string PName = value.ToString();

    //        if (PName.Length < 3)
    //        {
    //            return new ValidationResult("商品名稱至少三個字");
    //        }
    //        return ValidationResult.Success;
    //    }
    //}
}
