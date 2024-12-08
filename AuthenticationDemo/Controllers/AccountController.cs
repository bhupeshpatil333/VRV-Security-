using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthenticationDemo.Models;
using System.Web.Security;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace AuthenticationDemo.Controllers
{
    public class AccountController : Controller
    {
        dbEntities db = new dbEntities();
        // GET: Account
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(user u)
        {
            if(ModelState.IsValid)
            {
                db.users.Add(u);
                if(db.SaveChanges()>0)
                {
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(user u,string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = db.users.Where(x => x.username == u.username && x.password == u.password).FirstOrDefault();
                if(user!=null)
                {
                    FormsAuthentication.SetAuthCookie(u.username, false);
                    Session["uname"] = u.username.ToString();
                    if (ReturnUrl != null)
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return Redirect("/Home/Service");
                    }
                }
            }

            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["uname"] = null;
            return Redirect("Login");
        }

        public ActionResult UserDetails(int id)
        {
           
            var user = db.users.Find(id); 
            if (user == null)
            {
                return HttpNotFound(); 
            }
            return View(user); 
        }

        public ActionResult getUserList()
        {
            var users = db.users.ToList(); 
            return View(users); 
        }


        // GET: Edit
        public ActionResult Edit(int id)
        {
            var user = db.users.Find(id); 
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, user updatedUser)
        {
            if (ModelState.IsValid)
            {
                var user = db.users.Find(id);  
                if (user == null)
                {
                    return HttpNotFound();
                }

                
                user.username = updatedUser.username;
                user.password = updatedUser.password;
                
                db.SaveChanges();

                return RedirectToRoute(new { controller = "Home", action = "getUserList" });

            }

            return View(updatedUser);  
        }

        public ActionResult Delete(int id)
        {
            var user = db.users.Find(id); 
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            
            var roles = db.roles.Where(r => r.UserId == id).ToList();
            db.roles.RemoveRange(roles);  

            
            db.users.Remove(user);
            db.SaveChanges();  

           
            return RedirectToAction("getUserList", "Home"); 
        }





    }
}