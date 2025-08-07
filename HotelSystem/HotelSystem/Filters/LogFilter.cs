using Microsoft.AspNetCore.Mvc.Filters;

namespace HotelSystem.Filters
{
    public class LogFilter:IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Log the action execution start
            Console.WriteLine($"Action {context.ActionDescriptor.DisplayName} is starting at {DateTime.Now}");
        }


        //行為完成後，開始抓下列東西
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];
            var id = context.RouteData.Values["id"];

            var agent = context.HttpContext.Request.Headers["User-Agent"].ToString();
            var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();

            string user = "Guest";
            var time = DateTime.Now;

            string logMessage = $"{time} - {user} - {ip} - {agent} - {controller}/{action}/{id}";

            //寫入日誌系統
            var filePath = "LogFiles/ActionLog.txt";

            if (!Directory.Exists("LogFiles"))
            {
                Directory.CreateDirectory("LogFiles");// 確保目錄存在
            }

            //將日誌訊息寫入檔案
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(logMessage); //WriteLine寫完一行後，會自動一行
            }
        }
    }
}
