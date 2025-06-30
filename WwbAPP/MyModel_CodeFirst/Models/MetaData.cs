//namespace MyModel_CodeFirst.Models
//{

//}
//namespace中放入的類別會被視為同一個命名空間，這樣可以讓程式碼更有組織性和可讀性


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace MyModel_CodeFirst.Models;

public class BookData
{
    //物件導向中的「屬性封裝」
    [Display(Name = "留言編號")]
    [Required(ErrorMessage = "必填")]
    [Key]
    public string BookID { get; set; } //留言編號，採用GUID (4組16進位的字串編的，有128bit，唯一識別值的產生方式)

    [Display(Name = "留言主旨")]
    [StringLength(30, MinimumLength = 2, ErrorMessage = "主題2~30個字")]
    [Required(ErrorMessage = "必填")]
    public string Title { get; set; } //留言主旨

    [Display(Name = "留言內容")]
    [Required(ErrorMessage = "必填")]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; } //留言內容

    [Display(Name = "留言者")]
    [StringLength(20, ErrorMessage = "最多20字")]
    [Required(ErrorMessage = "必填")]
    public string Author { get; set; } //留言者

    [Display(Name = "照片")]
    [StringLength(40)]
    public string? Photo { get; set; } //照片檔名規則：GUID+.jpg
    //這個做法是指將圖片存在伺服器中的某個地方
    //byte[] PhotoBinary { get; set; } //圖片的二進位資料，直接將圖片轉成二進位制，丟到資料庫中

    [Display(Name = "發布時間")]
    [DataType(DataType.DateTime)] //給View看得，他在表單上的呈現
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")] //指定資料在View上顯示的格式，{0}表示顯示原始值，「:」後加上要指定的格式
    [HiddenInput] //隱藏欄位，這個欄位不會在表單填寫時顯示
    public DateTime CreateDate { get; set; } = DateTime.Now; //留言時間
}

public class ReBookData
{
    [Display(Name = "回覆編號")]
    [StringLength(36, MinimumLength = 36)]
    [Key]
    public string ReBookID { get; set; } //留言編號，採用GUID (4組16進位的字串編的，有128bit，唯一識別值的產生方式)

    [Display(Name = "回覆內容")]
    [Required(ErrorMessage = "必填")]
    public string Description { get; set; } //留言內容

    [Display(Name = "回覆者")]
    [StringLength(10, ErrorMessage = "最多10字")]
    [Required(ErrorMessage = "必填")]
    public string Author { get; set; } //留言者

    [Display(Name = "回覆時間")]
    [DataType(DataType.DateTime)] //給View看得，他在表單上的呈現
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")] //指定資料在View上顯示的格式，{0}表示顯示原始值，「:」後加上要指定的格式
    [HiddenInput] //隱藏欄位，這個欄位不會在表單填寫時顯示
    public DateTime CreateDate { get; set; } = DateTime.Now; //留言時間

    //1.1.5 撰寫兩個類別間的關聯屬性做為未來資料表之間的關聯

    //外來鍵屬性
    [ForeignKey("Book")]
    public string BookID { get; set; } = null!;
}

[ModelMetadataType(typeof(BookData))]
public partial class Book
{
}

[ModelMetadataType(typeof(ReBookData))]
public partial class ReBook
{
}