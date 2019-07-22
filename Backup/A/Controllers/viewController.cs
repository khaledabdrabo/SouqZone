using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace A.Controllers
{
    public class viewController : Controller
    {

        //////////////// Supllier //////////////////

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Products()
        {
            return View();
        }

        public ActionResult Profile()
        {
            return View();
        }

        public ActionResult ProductDetails()
        {
            return View();
        }

        //////////////// ShopOwner //////////////////

        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult ProductsMale()
        {
            return View();
        }
        public ActionResult ProductsFemale()
        {
            return View();
        }

        public ActionResult Profile2()
        {
            return View();
        }

        public ActionResult ProductDetails2()
        {
            return View();
        }

    }
}
