using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace CSharp.Controllers
{
    public class IDNumberController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public string CheckIDNumber(string ID)
        {
            

            //A123456789
            //格式檢查
            //1.長度必須是10個字元
            //2.第一個字元必須是英文字母
            //3.第二個字元必須1或2是數字
            //4.第三個字元必須是數字

            if (string.IsNullOrEmpty(ID))
                return "這請輸入身分證字號";

            if (ID.Length != 10)
                return "身分證字號長度不正確";

            string letter = "ABCDFEGHJKLMNPQRSTUVXYQZIO";

            if (letter.IndexOf(ID[0]) == -1)
                return "這是不合法的身分證字號";

            if (ID[1]!='1' && ID[1]!='2') //要用單引號，因是指在字串中尋找
                return "這是不合法的身分證字號";

            for (int i=2; i<ID.Length; i++)
            {
                if (ID[i]<'0' || ID[i]>'9')
                    return "這是不合法的身分證字號";
            }

            //////////////////////////////////////////////////
            //計算身分證字號的總和
            int n = 0; //用來計算身分證字號的總和

            int letterNum = letter.IndexOf(ID[0]) + 10; //字母轉數字

            int n1 = letterNum / 10; //十位數
            int n2 = letterNum % 10; //個位數

            
            n = n1 * 10 + n2 * 9;
            for (int k=1; k<= 8; k++)
            {
                n += (ID[k] - '0') * (9 - k);
            }

            

            return "n總數為" + n;
            if (n % 10 == 0)
                return "這是合法的身分證字號";
        }
    }
}
