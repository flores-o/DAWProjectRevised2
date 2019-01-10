using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class ProjectController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var projects = from project in db.Projects.Include("Organizer")
                           orderby project.Name
                           select project;
            ViewBag.Projects = projects;
            
            return View();
        }

        public ActionResult Show(int id)
        {
            Project project = db.Projects.Find(id);
            ViewBag.Project = project;
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Project project)
        {
            try
            {
                Console.WriteLine(project);
                //Modified
                project.OrganizerId = User.Identity.GetUserId();

                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:");
                Console.WriteLine(e);

                return View();
            }
        }
       //Modified
        [Authorize(Roles = "Organizer,Administrator")]        public ActionResult Edit(int id)
        {
            Project project = db.Projects.Find(id);
            ViewBag.Project = project;

            if (project.OrganizerId == User.Identity.GetUserId() ||
           User.IsInRole("Administrator"))
            {
                return View(project);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui proiect care nu va apartine!";
          return RedirectToAction("Index");
            }
        }


        [Authorize(Roles = "Organizer,Administrator")]
        [HttpPut]
        public ActionResult Edit(int id, Project requestProject)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Project project = db.Projects.Find(id);
                    if (project.OrganizerId == User.Identity.GetUserId() ||
                   User.IsInRole("Administrator"))
                    {
                        if (TryUpdateModel(project))
                        {
                            project.Name = requestProject.Name;
                            project.Description = requestProject.Description;
                            db.SaveChanges();
                            TempData["message"] = "Proiectul a fost modificat!";
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui proiect care nu va apartine!";
                     return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View();
                }

            }
            catch (Exception e)
            {
                return View();
            }
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}