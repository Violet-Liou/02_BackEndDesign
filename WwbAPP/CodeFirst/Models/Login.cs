using System.ComponentModel.DataAnnotations;

namespace CodeFirst.Models
{
    public class Login
    {
        //★Model寫得越完整，view就不用改太多

        [Display(Name = "帳號")]
        [StringLength(10, MinimumLength =5, ErrorMessage = "帳號為5-10碼")]
        [Key]
        [Required(ErrorMessage = "必填")]
        [RegularExpression("[A-Za-z][A-Za-z0-9_]{4,9}")]
        // 正規表示式：帳號必須以字母開頭，後面可以跟字母、數字或底線，長度在5到10個字元之間
        //[A-Za-z] 第一碼必須為英文字
        //[A-Za-z0-9_] 後面可以是英文字、數字或底線，{4,9}長度在4到9個字元之間
        public string Account { get; set; } = null!; // 使用 null! 來避免非空警告，因為這個屬性會在登入時被賦值

        [Display(Name = "密碼")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "密碼為8-20碼")]
        [Required(ErrorMessage = "必填")]
        [MaxLength(20)]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
