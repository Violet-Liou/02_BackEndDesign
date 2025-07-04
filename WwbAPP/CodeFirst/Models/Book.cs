using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirst.Models
{
    public partial class Book
    {
        public string BookID { get; set; } //留言編號，採用GUID (4組16進位的字串編的，有128bit，唯一識別值的產生方式)

        public string Title { get; set; } //留言主旨

        public string Description { get; set; } //留言內容

        public string Author { get; set; } //留言者

        public string? Photo { get; set; } //照片檔名規則：GUID+.jpg
        //這個做法是指將圖片存在伺服器中的某個地方
        //byte[] PhotoBinary { get; set; } //圖片的二進位資料，直接將圖片轉成二進位制，丟到資料庫中

        public DateTime CreateDate { get; set; } = DateTime.Now; //留言時間

        //1.1.5 撰寫兩個類別間的關聯屬性做為未來資料表之間的關聯
        public virtual List<ReBook>? ReBooks { get; set; } //留言的回覆，這個屬性是用來存放與此書籍相關的留言回覆
    }
}
