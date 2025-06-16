using Microsoft.AspNetCore.Mvc;

namespace CSharp.Controllers
{
    public class CSharpController : Controller
    {
        //在類別裡的一個方法，他叫Action
        public IActionResult Index()
        {
            return View(); //MVC模型中，View永遠是被return的，因為模型中V是要response給Client的
        }

        public string Hello()
        {
            return "Hello World!";
        }

        public void Number()
        {
            int a = 123; //32位帶正負號整數
            short b = 123; //16位帶正負號整數
            long c = 123; //64位帶正負號整數

            float d = 123.45f; //單精準度(32位帶正負號)浮點數
            double e = 123.45; //倍精準度(64位帶正負號)浮點數
        }

        public void Var()
        {
            string a = "123"; //字串
            bool b = true;
        }

        public string Stament()
        {
            int a = 123;
            a += 10;
            a++;

            return "a變數的值為" + a; //字串拼接
        }
    }
}