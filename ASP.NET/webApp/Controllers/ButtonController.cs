using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace webApp.Controllers
{
    public class ButtonController : Controller
    {

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult HandleButtonClick(string myBtn)
        {

            return View("Index");
        }
    }
}
