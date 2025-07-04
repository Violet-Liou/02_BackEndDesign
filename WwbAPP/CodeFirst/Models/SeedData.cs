using CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Models
{
    public class SeedData
    {
        //1.3.3 撰寫SeedData類別的內容
        //  (1) 撰寫靜態方法 Initialize(IServiceProvider serviceProvider)
        public static void Initialize(IServiceProvider serviceProvider)
        {
            //(2) 撰寫Book及ReBook資料表內的初始資料程式
            using (GuestBookContext context =
                new GuestBookContext(serviceProvider.GetRequiredService<DbContextOptions<GuestBookContext>>()))   //用dbContext的建構子取得資料庫連線
            {
                //(4)加上 using () 及 判斷資料庫是否有資料的程式
                if (!context.Book.Any())
                {
                    string[] guid = [Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),];
                    context.Book.AddRange(
                        new Book
                        {
                            BookID = guid[0],
                            Title = "第一篇留言",
                            Description = "這是第一篇留言的內容",
                            Author = "作者A",
                            Photo = guid[0] + ".jpg",
                            CreateDate = DateTime.Now
                        },
                        new Book
                        {
                            BookID = guid[1],
                            Title = "第二篇留言",
                            Description = "這是第二篇留言的內容",
                            Author = "作者B",
                            Photo = guid[1] + ".jpg",
                            CreateDate = DateTime.Now
                        },
                        new Book
                        {
                            BookID = guid[2],
                            Title = "第三篇留言",
                            Description = "這是第三篇留言的內容",
                            Author = "作者C",
                            Photo = guid[2] + ".jpg",
                            CreateDate = DateTime.Now
                        },
                        new Book
                        {
                            BookID = guid[3],
                            Title = "第四篇留言",
                            Description = "這是第四篇留言的內容",
                            Author = "作者D",
                            Photo = guid[3] + ".jpg",
                            CreateDate = DateTime.Now
                        },
                        new Book
                        {
                            BookID = guid[4],
                            Title = "第五篇留言",
                            Description = "這是第五篇留言的內容",
                            Author = "作者E",
                            Photo = guid[4] + ".jpg",
                            CreateDate = DateTime.Now
                        }
                    );

                    context.ReBook.AddRange(
                        new ReBook
                        {
                            ReBookID = Guid.NewGuid().ToString(),
                            Description = "這是第一篇留言的內容",
                            Author = "作者A",
                            CreateDate = DateTime.Now,
                            BookID = guid[0]
                        },
                        new ReBook
                        {
                            ReBookID = Guid.NewGuid().ToString(),
                            Description = "這是第五篇留言的內容",
                            Author = "作者E",
                            CreateDate = DateTime.Now,
                            BookID = guid[4]
                        }
                    );
                    context.SaveChanges(); //儲存變更到資料庫

                    //(3)撰寫上傳圖片的程式
                    string SeedPhotosPath = Path.Combine(Directory.GetCurrentDirectory(), "img");//取得來源照片路徑
                    string BookPhotosPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BookPhotos");//取得目的路徑


                    string[] files = Directory.GetFiles(SeedPhotosPath);  //取得指定路徑中的所有檔案

                    for (int i = 0; i < files.Length; i++)
                    {
                        string destFile = Path.Combine(BookPhotosPath, guid[i] + ".jpg");


                        File.Copy(files[i], destFile);
                    }
                }
            }//using結束
        }
    }
}
