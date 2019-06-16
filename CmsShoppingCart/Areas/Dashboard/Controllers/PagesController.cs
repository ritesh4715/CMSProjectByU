using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShoppingCart.Areas.Dashboard.Controllers
{
    public class PagesController : Controller
    {
        // GET: Dashboard/Pages
        public ActionResult Index()
        {
            return View();
        }
    }
}