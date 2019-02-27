using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using darkZencefil.Models;

//bunlar sayfalama işlemi için gerekn kütüphaneler
using PagedList;
using PagedList.Mvc;


namespace darkZencefil.Controllers
{
    public class siteController : Controller
    {
        veriTabani db = new veriTabani();
        public ViewResult anasayfa()
        {
            return View();
        }
        
        public ActionResult blog(int id, int ? i)
        {

            //sayfalama yapmadan önce
            //var yazi = db.Yazi.Where(y => y.KategoriID == id).ToList();
            //return View(yazi);

            //toPaget list 2 parametre alır.1.si neye göre sıralama yapcak     2. si ise  ka. tane sıralayacak
            //   var yazi = db.Yazi.Where(y => y.KategoriID == id).ToPagedList(i ?? 1, 2);

            //var yazi = db.Yazi.OrderByDescending(y => y.KategoriID == id).ToList().ToPagedList(i ?? 1, 3);

           

         

            var yazı = db.Yazi.Where(y => y.KategoriID == id).ToList().ToPagedList(i ?? 1, 3);

      
            return View(yazı);
        }

        //public ActionResult haberler(int SayfaNo)
        //{


        //    int _sayfaNo = 1;
        //    var MusteriListesi = db.Yazi.OrderByDescending(y => y.KategoriID == blogId).ToPagedList(_sayfaNo, 2);


        //    //if (Request.IsAjaxRequest())
        //    //{
        //    //    return PartialView("~/Views/Home/_MusteriListesi.cshtml", MusteriListesi);
        //    //}

        //    return View(MusteriListesi);


        //    //var yazi = db.Yazi.Where(y => y.KategoriID == id).ToList();
        //    //return View(yazi);
        //}

        public ViewResult iletisim()
        {
            return View();
        }
        public ActionResult KategoriPartial1()
        {
            return View(db.Kategori.ToList());
        }
        public ActionResult KategoriPartial2()
        {
            return View(db.Kategori.ToList());
        }
        public ActionResult SonYazilar()
        {
            return View(db.Yazi.OrderByDescending(y => y.Tarih).Take(5));
        }
        public ActionResult BlogArama(string Ara = null)
        {
            var aranan = db.Yazi.Where(y => y.Baslik.Contains(Ara)).ToList();
            return View(aranan.OrderByDescending(y => y.Tarih));
        }
        public ActionResult sizeözel(int id)
        {
            var yazi = db.Yazi.Where(y => y.KategoriID == id).ToList();
            return View(yazi);
        }
        public ActionResult haberDetay(int id)
        {
            var yazi = db.Yazi.Where(y => y.YaziID == id).SingleOrDefault();
            if (yazi == null)
            {
                return HttpNotFound();
            }
            return View(yazi);
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            //böyle bir cookiemiz var mı diye soruyoruz
            if (GetCookie(actionName) == null)
            {
                //yoksa yeni bir cookie oluştuyoruz
                CreateCookie(actionName, yazi.Kategori.KategoriAdi);
            }
            else
            {
                CreateCookie(actionName, yazi.Kategori.KategoriAdi);      
            }
        }
        public ActionResult YorumSil(int id)
        {
            var uyeid = Session["uyeid"];
            var yorum = db.Yorum.Where(y => y.YorumID == id).SingleOrDefault();
            var yazi = db.Yazi.Where(y => y.YaziID == yorum.YaziID).SingleOrDefault();
            if (yorum.UyeID == Convert.ToInt32(uyeid))
            {
                db.Yorum.Remove(yorum);
                db.SaveChanges();
                return RedirectToAction("haberDetay", "site", new { id = yazi.YaziID });
            }
            else
            {
                return HttpNotFound();
            }
        }
        [HttpPost]
        public JsonResult YorumYap(string yorum, int yaziID) // veri bakımından küçük olan json lar sayfa yenilenmeden eklenir.
        {
            var uyeID = Session["uyeid"];
            if (yorum != null)
            {
                db.Yorum.Add(new Yorum { UyeID = Convert.ToInt32(uyeID), YaziID = yaziID, Metin = yorum, Tarih = DateTime.Now });
                db.SaveChanges();
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public ActionResult slider()
        {
            return View(db.Yazi.OrderByDescending(y => y.YaziID).Take(5));
        }
        public ActionResult profil(string kulAdi)
        {
            var profilBilgisi = db.Uye.Where(y => y.KullaniciAdi == kulAdi).SingleOrDefault();
            if (profilBilgisi == null)
            {
                return HttpNotFound();
            }
            return View(profilBilgisi);
        }
        public ActionResult deneme(string id)
        {
            var yazi = db.Uye.Where(y => y.KullaniciAdi == id).SingleOrDefault();
            if (yazi == null)
            {
                return HttpNotFound();
            }
            return View(yazi);
        }
        public ActionResult  kullaniciProfil(string id)
        {
            var uye = db.Uye.Where(u => u.KullaniciAdi == id).SingleOrDefault();
            return View(uye);
        }
        private void CreateCookie(string name, string value)
        {
            HttpCookie cookieVisitor = new HttpCookie(name, value);
            // cookieVisitor.Expires = DateTime.Now.AddDays(2);
            Response.Cookies.Add(cookieVisitor);
        }
        private string GetCookie(string name)
        {
            //Böyle bir cookie mevcut mu kontrol ediyoruz
            if (Request.Cookies.AllKeys.Contains(name))
            {
                //böyle bir cookie varsa bize geri değeri döndürsün
                return Request.Cookies[name].Value;
            }
            return null;
        }
        private void DeleteCookie(string name)
        {
            //Böyle bir cookie var mı kontrol ediyoruz
            if (GetCookie(name) != null)
            {
                //Varsa cookiemizi temizliyoruz
                Response.Cookies.Remove(name);
                //ya da 
                Response.Cookies[name].Expires = DateTime.Now.AddDays(-1);
            }
        }
        public ActionResult Index()
        {
            //Bulunduğumuz sayfanının actionname ni alıyoruz
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            //böyle bir cookiemiz var mı diye soruyoruz
            if (GetCookie(actionName) == null)
            {
                //yoksa yeni bir cookie oluştuyoruz
                CreateCookie(actionName, "visitor");
            }
            else
            {
                //Ekranda görünmesini sitediğimiz mesajımız
                ViewBag.Message = "Sen daha önce bu sayfayı ziyaret etmişsin :) Tekrar Ziyaretiniz için teşekkür ederiz.";
            }
            return View();
        }

        public ActionResult About()
        {
            //Bulunduğumuz sayfanının actionname ni alıyoruz
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            //böyle bir cookiemiz var mı diye soruyoruz
            if (GetCookie(actionName) == null)
            {
                //yoksa yeni bir cookie oluştuyoruz
                CreateCookie(actionName, "visitor");
            }
            else
            {
                //Ekranda görünmesini sitediğimiz mesajımız
                ViewBag.Message = "Sen daha önce bu sayfayı ziyaret etmişsin :)";
            }

            return View();
        }

    }
}