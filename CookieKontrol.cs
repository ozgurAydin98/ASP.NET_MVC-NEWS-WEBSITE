using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace darkZencefil
{
    public class CookieKontrol
    {
        static public void CookieKaydet()
        {
            HttpCookie Cookie = null;
            if (HttpContext.Current.Response.Cookies["OrnekCookie"] != null)
                //Cookie varsa devam.
                Cookie = HttpContext.Current.Response.Cookies["OrnekCookie"];
            else
                //Yoksa oluşturuyoruz.
                Cookie = new HttpCookie("OrnekCookie");

            Cookie.Expires = DateTime.Now.AddDays(3);

            Cookie["Veri1"] = "Veri1";
            //Cookie["Veri2"] = "Veri2";
            //Cookie["Veri3"] = "Veri3";

            HttpContext.Current.Response.Cookies.Add(Cookie);
        }

        static public HttpCookie CookieGetir()
        {
            if (HttpContext.Current.Request.Cookies["OrnekCookie"] != null)
                return HttpContext.Current.Request.Cookies["OrnekCookie"];
            else
                return null;
        }

        static public void CookieSil() => HttpContext.Current.Response.Cookies["OrnekCookie"].Expires = DateTime.Now.AddDays(-1);





    }
}