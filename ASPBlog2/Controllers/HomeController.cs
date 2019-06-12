using ASPBlog2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static DataLibrary.Logik.UsersDB;

namespace ASPBlog2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(UsersModel model)
        {
            if (ModelState.IsValid)
            {
                createUser(model.age, model.name, model.lastname, model.email, model.password);
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult viewUsers()
        {
            var data = loadUsers();
            List<UsersModel> user = new List<UsersModel>();

            foreach(var i in data)
            {
                user.Add(new UsersModel
                {
                    age = i.age,
                    name = i.name,
                    lastname = i.lastname,
                    email = i.email,
                    password = i.password
                });
            }

            return View(user);
        }
    }
}