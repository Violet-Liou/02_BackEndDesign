using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Models
{
    //1.2.2 撰寫GuestBookContext類別的內容
    //(1)需繼承DbContext類別
    public class GuestBookContext : DbContext
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


        //1.2.6 在DbContext中使用Fluent API在GuestBookContext覆寫 OnModelCreating 方法
        //public 大家都可以使用
        //private 只有這個類別可以使用
        //protected 有繼承的類別也可以使用
        //void 沒有任何回傳值的函數
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //這裡可以進行資料表的配置 >>將資料表設定做成函數
            //例如：設定主鍵、索引、關聯等
            //modelBuilder.Entity<Book>()
            //    .HasKey(b => b.BookID); //設定Book資料表的主鍵
            //modelBuilder.Entity<ReBook>()
            //    .HasKey(r => r.ReBookID); //設定ReBook資料表的主鍵
            //modelBuilder.Entity<ReBook>()
            //    .HasOne(r => r.Book) //ReBook與Book之間是一對多的關係
            //    .WithMany(b => b.ReBooks) //Book可以有多個ReBook
            //    .HasForeignKey(r => r.BookID); //設定外來鍵

            //以上，是AI撰寫
            //以下，老師教的


            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.BookID); //設定Book資料表的主鍵

                entity.Property(e => e.BookID)
                    .IsUnicode(false) //不使用Unicode編碼 >> 因為使用GUID，只有英數字，所以不使用Unicode
                    .HasMaxLength(36) //BookID最大長度為36
                    .IsRequired(); //必填

                entity.Property(e => e.Title)
                    .IsRequired() //必填
                    .HasMaxLength(30); //Title最大長度為30

                entity.Property(e => e.Description)
                    .IsRequired(); //必填

                entity.Property(e => e.Author)
                    .IsRequired() //必填
                    .HasMaxLength(20); //Author最大長度為20

                entity.Property(e => e.Photo)
                    .HasMaxLength(40); //Author最大長度為20

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime"); //指定CreateDate的資料型別為datetime
            });

            modelBuilder.Entity<ReBook>(entity =>
            {
                entity.HasKey(e => e.ReBookID); //設定ReBook資料表的主鍵

                entity.Property(e => e.ReBookID)
                    .IsUnicode(false) //不使用Unicode編碼 >> 因為使用GUID，只有英數字，所以不使用Unicode
                    .HasMaxLength(36) //BookID最大長度為36
                    .IsRequired(); //必填

                entity.Property(e => e.Description)
                    .IsRequired(); //必填

                entity.Property(e => e.Author)
                    .IsRequired() //必填
                    .HasMaxLength(20); //Author最大長度為20

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime"); //指定CreateDate的資料型別為datetime
            });
        }

    }
}
