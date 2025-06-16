using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyView.Models;

namespace MyView.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        //�i��k�@�j
        ////�ΰ}�C�Ӧs��ժO�W�������
        //string[] id = { "A01", "A02", "A03", "A04", "A05", "A06", "A07" };
        //string[] name = { "���ש]��", "�sԳ���Ӱ�", "���X�]��", "�C�~�]��", "���]��", "�j�F�]��", "�Z�t�]��" };

        //string[] address = { "813����������ϸθ۸�", "800�������s���ϥɿŨ�", "800�x�W�������s���Ϥ��X�G��",
        //        "80652�������e��ϳͱۥ|��758��", "�x�n���_�Ϯ��w���T�q533��", "�x�n���F�ϪL�˸��@�q276��",
        //        "�x�n������ϪZ�t��69��42��" };
        //public IActionResult Index()
        //{
        //    List<NightMarket> list = new List<NightMarket>(); //�x������

        //    for(int i=0; i <id.Length; i++)
        //    {
        //        NightMarket nm = new NightMarket();
        //        nm.Id = id[i];
        //        nm.Name = name[i];
        //        nm.Address = address[i];
        //        list.Add(nm);
        //    }
        //    return View(list);
        //}
        //public IActionResult IndexRWD()
        //{
        //    List<NightMarket> list = new List<NightMarket>(); //�x������

        //    for (int i = 0; i < id.Length; i++)
        //    {
        //        NightMarket nm = new NightMarket();
        //        nm.Id = id[i];
        //        nm.Name = name[i];
        //        nm.Address = address[i];
        //        list.Add(nm);
        //    }
        //    return View(list);
        //}

        //�i��k�G�j
        private List<NightMarket> GetData()
        {
            string[] id = { "A01", "A02", "A03", "A04", "A05", "A06", "A07" };
            string[] name = { "���ש]��", "�sԳ���Ӱ�", "���X�]��", "�C�~�]��", "���]��", "�j�F�]��", "�Z�t�]��" };

            string[] address = { "813����������ϸθ۸�", "800�������s���ϥɿŨ�", "800�x�W�������s���Ϥ��X�G��",
                "80652�������e��ϳͱۥ|��758��", "�x�n���_�Ϯ��w���T�q533��", "�x�n���F�ϪL�˸��@�q276��",
                "�x�n������ϪZ�t��69��42��" };

            List<NightMarket> list = new List<NightMarket>(); //�x������

            for (int i = 0; i < id.Length; i++)
            {
                NightMarket nm = new NightMarket();
                nm.Id = id[i];
                nm.Name = name[i];
                nm.Address = address[i];
                list.Add(nm);
            }
            return list;
        }

        public IActionResult Index()
        {
            return View(GetData());
        }
        public IActionResult IndexRWD()
        {
            return View(GetData());
        }

        public IActionResult Detail(string id)
        {
            //List<NightMarket> list = GetData();
            var list = GetData(); //�ϥ�var����r�A�|�۰ʱ��_���O

            //select *
            //from list
            //where id=""

            //Linq�y�k
            //var result= (from n in list
            //             where n.Id == id
            //             select n).FirstOrDefault();

            //Lambda�y�k
            var result = list.Where(list => list.Id == id).FirstOrDefault();

            return View(result);
        }

        public IActionResult IndexList(string id)
        {
            //�i��k�@�j�ۤv��e��
            //var list = GetData(); //�ϥ�var����r�A�|�۰ʱ��_���O

            ////���������C
            ////���o�Ҧ��]����ƪ��s���P�W��
            //ViewData["nm"] = list;

            ////�k����ܸ�Ƥ��e�D�e��
            ////���o�Y�@���]����ƪ��ԲӤ��e

            //var result = list.Where(list => list.Id == id).FirstOrDefault();

            //return View(result);

            //�i��k�G�j�ϥ�ViewModel�A���Φۤv��e��
            var list = GetData();
            VMNightMarket vmM = new VMNightMarket();
            {
                vmM.NightMarkets = list; //�Ω�������C���h�����
                vmM.NightMarket = list.Where(n => n.Id == id).FirstOrDefault(); //�Ω�k����ܪ��涵���
            }
            return View(vmM);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Razor()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
