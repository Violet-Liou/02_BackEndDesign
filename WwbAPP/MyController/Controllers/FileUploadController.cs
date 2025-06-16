using Microsoft.AspNetCore.Mvc;

namespace MyController.Controllers
{
    public class FileUploadController : Controller
    {
        public IActionResult ShowPhoto()
        {
            //要wwwroot這個跟目錄下，這個images資料夾的路徑(這種是較活用的，不要用寫死的方法wwwroot/Photos/xxx.jpg)
            String[] files= Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images"));

            foreach (String file in files)
            {
                //取得檔案名稱
                String fileName = Path.GetFileName(file);
                ViewData["Photos"] += $"<img src='/images/{fileName}'/>";
                ////取得檔案的完整路徑
                //String filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                ////wwwroot/Photos/xxx.jpg
                ////將檔案上傳並儲存於指定的路徑 (使用複製檔案的方式，去將檔案儲存下來)
                //using (FileStream fs = new FileStream(filePath, FileMode.Create))
                //{
                //    //photo.CopyTo(fs);
                //}
            }

            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(IFormFile photo) //IFormFile 是一個介面 (i開頭的都是介面)
        {
            if(photo == null || photo.Length == 0)
            {
                ViewData["ErrMessage"] = "請選擇檔案上傳，你根本沒有上傳壓~~";
                return View();
            }

            //只允許上傳圖片
            if(photo.ContentType !="image/jpeg" && photo.ContentType != "image/png")
            {
                ViewData["ErrMessage"] = "只允許上傳圖片!!";
                return View();
            }

            //取得檔案名稱
            String fileName = Path.GetFileName(photo.FileName);

            //取得檔案的完整路徑
            String filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
            //wwwroot/Photos/xxx.jpg

            //將檔案上傳並儲存於指定的路徑 (使用複製檔案的方式，去將檔案儲存下來)
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            { 
                photo.CopyTo(fs);
            }
            return View();
        }
    }
}
