using Microsoft.EntityFrameworkCore;

namespace MyModel_CodeFirst.Models
{
    //1.2.2 撰寫GuestBookContext類別的內容
    //(1)需繼承DbContext類別
    public class GuestBookContext:DbContext
    {
        //一般建構子的寫法：很像方法，沒有返回值
        //public GuestBookContext()
        //{

        //}


        //(2)撰寫依賴注入用的建構子
        //依賴注入的建構子寫法
        public GuestBookContext(DbContextOptions<GuestBookContext> options)
            : base(options) //這裡的base是指父類別DbContext的建構子
        {

        }

        //(3) 描述資料庫裏面的資料表 (用程式寫出資料表的結構)
        public virtual DbSet<Book> Book { get; set; } //Book資料表
        public virtual DbSet<ReBook> ReBook { get; set; } //ReBook資料表
        //virtual ：指不是實際的資料表

    }
}
