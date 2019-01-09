using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        // GET: Users
        public ActionResult Index()
        {   //How are Users called in our app
            var users = from user in db.Users
                        orderby user.UserName
                        select user;
            ViewBag.UsersList = users;
            return View();
        }
    }
}