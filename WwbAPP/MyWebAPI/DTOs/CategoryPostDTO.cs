using System.ComponentModel.DataAnnotations;
using MyWebAPI.Models;

namespace MyWebAPI.DTOs
{
    //5.3.6 建立CategoryPostDTO類別
    public class CategoryPostDTO
    {
        //5.3.7 在CategoryPostDTO.cs加入需要的內建驗證器(Validator)
        [Required]
        [RegularExpression("[A-Z][1-9]")]
        public string CateID { get; set; } = null!;

        [Required]
        [StringLength(20)]
        //5.3.9 在需要使用此驗證器的屬性上加入標籤
        [CateDulicateCheck]
        public string CateName { get; set; } = null!;
    }

    //5.3.8 在CategoryPostDTO.cs加入自訂驗證器(使用ValidationAttribute物件)
    public class  CateDulicateCheck:ValidationAttribute
    {
        //建構物件【測試二】
        //private readonly GoodStoreContextG2 _context;
        //public CateDulicateCheck(GoodStoreContextG2 context)
        //{
        //    //建構物件
        //    _context = context;
        //}
        //不可以模仿Controller的寫法注入Context >> 因為在上面要使用自訂驗證器的時候，他會要你傳參數(因為有建構子)

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string CName = value.ToString();

            //建構物件【測試一】
            //GoodStoreContextG2 _context = new GoodStoreContextG2(); 
            //因為GoodStoreContextG2沒有建構子，所以無法直接用new的方式使用

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
}
