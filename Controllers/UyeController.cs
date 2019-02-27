using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using darkZencefil.Models;
namespace darkZencefil.Controllers
{
    public class UyeController : Controller
    {
        veriTabani db = new veriTabani();
        // GET: Uye
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Login(Uye uye)
        {
            try
            {
                var login = db.Uye.Where(u => u.KullaniciAdi == uye.KullaniciAdi).SingleOrDefault();
                if (login.KullaniciAdi == uye.KullaniciAdi && login.Sifre == uye.Sifre)
                {
                    Session["uyeid"] = login.UyeID;
                    Session["kullaniciadi"] = login.KullaniciAdi;
                    Session["yetki"] = login.Yetki;
                    return RedirectToAction("anasayfa", "Site");
                }
                else { return View(); }
            }
            catch (Exception e)
            {
                return RedirectToAction("/Uye", "/Login");

            }

        }
        public ActionResult Logout()
        {
            Session["uyeid"] = null;
            Session.Abandon();
            return RedirectToAction("anasayfa", "site");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Uye uye)
        {
            uye.Yetki = 0;
            if (ModelState.IsValid)
            {

                db.Uye.Add(uye);

                db.SaveChanges();
                return RedirectToAction("anasayfa", "site");
            }
            return View();
        }

    }
}