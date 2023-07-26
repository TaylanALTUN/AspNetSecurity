using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using System.Diagnostics;
using System.Text.Encodings.Web;
using XSS.Web.Models;

namespace XSS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private HtmlEncoder _htmlEncoder;
        private JavaScriptEncoder _javascriptEncoder;
        private UrlEncoder _urlEncoder;

        public HomeController(ILogger<HomeController> logger, HtmlEncoder htmlEncoder, JavaScriptEncoder javascriptEncoder, UrlEncoder urlEncoder)
        {
            _logger = logger;
            _htmlEncoder = htmlEncoder;
            _javascriptEncoder = javascriptEncoder;
            _urlEncoder = urlEncoder;
        }

        public IActionResult CommentAdd()
        {
            HttpContext.Response.Cookies.Append("email", "taylanaltun@gmail.com");
            HttpContext.Response.Cookies.Append("password", "1234");

            if(System.IO.File.Exists("comment.txt"))
            {
                ViewBag.comments = System.IO.File.ReadAllLines("comment.txt");
            }
            return View();
        }

        // Antiforgery kontrolünü kaldırır
        //[IgnoreAntiforgeryToken]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult CommentAdd(string name, string comment)
        {
            //url de gönderilecekse gelen datayı url de zararsız hale getirir
            //string urlEncodeName=_urlEncoder.Encode(name);
          
            ViewBag.Name = name;

            //Reflected XSS
            //richtextbox kullanılıyorsa direk ekrana yazdırma. Zararlı kodları temizle
            //< script > alert(document.cookie) </ script >
            // Zararlı html taglerini temizlewmek için hazır kütüphaneler mevcut. Aşağıdaki linkteki gibi 
            //https://github.com/mganss/HtmlSanitizer
            ViewBag.Comment = comment;


            //Stored XSS
            System.IO.File.AppendAllText("comment.txt", $"{name}-{comment}\n");


            return RedirectToAction("CommentAdd");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
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