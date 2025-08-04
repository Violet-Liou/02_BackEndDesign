using MyWebAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.ValidationAttributes
{
    public class MyValidator
    {
        public class CateDulicateCheck : ValidationAttribute
        {
            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                string CName = value.ToString();

                //建構物件【最終成功方法】!!!
                //從ValidationContext取得GoodStoreContextG2的實體
                GoodStoreContextG2 _context = validationContext.GetService<GoodStoreContextG2>();

                var result = _context.Category.Where(c => c.CateName == CName);

                if (result.Any())
                {
                    return new ValidationResult("商品名稱不能重複");
                }
                return ValidationResult.Success;
            }
        }

        public class ProductNameCheck : ValidationAttribute //因為是驗證器的class，所以需要繼承「ValidationAttribute」
        {
            //ValidationResult >> 驗證結果
            //ActionResult >> 動作結果
            protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
            {
                string PName = value.ToString();

                if (PName.Length < 3)
                {
                    return new ValidationResult("商品名稱至少三個字");
                }
                return ValidationResult.Success;
            }
        }
    }
}
