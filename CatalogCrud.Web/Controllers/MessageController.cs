using System.Web.Mvc;

namespace CatalogCrud.Web.Controllers
{
    public class MessageController : Controller
    {
        public ActionResult Error(string message = "Ошибка")
        {
            ViewBag.Message = message;
            return View();
        }

        public ActionResult PartialError(string message = "Ошибка")
        {
            ViewBag.Message = message;
            return PartialView();
        }
    }
}