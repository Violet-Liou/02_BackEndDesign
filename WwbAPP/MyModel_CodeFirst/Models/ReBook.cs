using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace MyModel_CodeFirst.Models
{
    public partial class ReBook
    {
        public string ReBookID { get; set; } //留言編號，採用GUID (4組16進位的字串編的，有128bit，唯一識別值的產生方式)

        public string Description { get; set; } //留言內容

        public string Author { get; set; } //留言者

        public DateTime CreateDate { get; set; } = DateTime.Now; //留言時間

        //1.1.5 撰寫兩個類別間的關聯屬性做為未來資料表之間的關聯
        public string BookID { get; set; } = null!;

        public virtual Book? Book { get; set; } //這個屬性是用來存放與此回覆相關的書籍，這樣可以建立書籍和回覆之間的關聯
    }
}
