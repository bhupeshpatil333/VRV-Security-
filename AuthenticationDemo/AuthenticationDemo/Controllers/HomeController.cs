using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthenticationDemo.Models;
namespace AuthenticationDemo.Controllers
{
 
    public class HomeController : Controller
    {
        dbEntities db = new dbEntities();
        // GET: Home
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles ="Admin")]
        public ActionResult getUserList()
        {
            var user = db.users.ToList();
            return View(user);
            
        }
        [Authorize]
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Service()
        {
            return View();
        }
    }
}