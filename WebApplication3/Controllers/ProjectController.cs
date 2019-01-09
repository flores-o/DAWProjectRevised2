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

        public ActionResult Edit(int id)
        {
            Project project = db.Projects.Find(id);
            ViewBag.Project = project;
            return View();
        }

        [HttpPut]
        public ActionResult Edit(int id, Project requestProject)
        {
            try
            {
                Project project = db.Projects.Find(id);
                if (TryUpdateModel(project))
                {
                    project.Name = requestProject.Name;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
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