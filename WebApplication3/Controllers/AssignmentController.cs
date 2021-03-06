﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class AssignmentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            int NOT_STARTED_STATUS = 1;
            int IN_PROGRESS_STATUS = 2;
            int COMPLETED_STATUS = 3;

            /*var assignments = from assignment in db.Assignments
                           orderby assignment.Name
                           select assignment;
            */

            var notStartedAssignments = from assignment in db.Assignments.Include("Comments").Include("Assignee")
                                        where assignment.Status == NOT_STARTED_STATUS
                                        select assignment;

            var inProgressAssignments = from assignment in db.Assignments.Include("Comments").Include("Assignee")
                                        where assignment.Status == IN_PROGRESS_STATUS
                                        select assignment;

            var completedAssignments = from assignment in db.Assignments.Include("Comments").Include("Assignee")
                                       where assignment.Status == COMPLETED_STATUS
                                       select assignment;

            //ViewBag.Assignments = assignments;
            ViewBag.NotStartedAssignments = notStartedAssignments;
            ViewBag.InProgressAssignments = inProgressAssignments;
            ViewBag.CompletedAssignments = completedAssignments;
            ViewBag.LoggedUserId = User.Identity.GetUserId();
            if(User.IsInRole("Administrator")) {
                ViewBag.UserIsAdmin = true;
            }

            return View();
        }

        public ActionResult Show(int id)
        {
            Assignment assignment = db.Assignments.Find(id);
            ViewBag.Assignment = assignment;
            return View();
        }

        [Authorize(Roles = "Organizer,Administrator")]

        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Organizer,Administrator")]

        [HttpPost]
        public ActionResult Create(Assignment assignment)
        {
            try
            {
                Console.WriteLine(assignment);

                db.Assignments.Add(assignment);
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

        [Authorize(Roles = "Administrator")]

        public ActionResult Edit(int id)
        {
            Assignment assignment = db.Assignments.Find(id);
            ViewBag.Assignment = assignment;
            return View();
        }

        [Authorize(Roles = "Administrator")]

        [HttpPut]
        public ActionResult Edit(int id, Assignment requestAssignment)
        {
            try
            {
                Assignment assignment = db.Assignments.Find(id);
                if (TryUpdateModel(assignment))
                {
                    assignment.Name = requestAssignment.Name;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize(Roles = "Organizer,Administrator")]

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Assignment assignment = db.Assignments.Find(id);
            db.Assignments.Remove(assignment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Adaugare comentariu
        public ActionResult AddComment(int id)
        {
            ViewBag.AssignmentId = id;
            ViewBag.AuthorId = User.Identity.GetUserId();

            Assignment assignment = db.Assignments.Find(id);
            ViewBag.TaskName = assignment.Name;
            return View();
        }

        [HttpPost]
        public ActionResult AddComment(Comment comment)
        {
            try
            {
                db.Comments.Add(comment);
                db.SaveChanges();

                return Redirect("/Assignment/Index");
                //return Redirect("/Assignment/Show/" + comment.AssignmentId);
            }
            catch (Exception e)
            {
                ViewBag.AssignmentId = comment.AssignmentId;
                return View();
            }
        }

        [HttpPut]
        public ActionResult ChangeStatus(int id, int status)
        {
            try
            {
                Assignment assignment = db.Assignments.Find(id);

                if (TryUpdateModel(assignment))
                {
                    assignment.Status = status;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }


        [Authorize(Roles = "Organizer,Administrator")]
        public ActionResult AssignTask(int id)
        {
            ViewBag.AssignmentId = id;

            Assignment assignment = db.Assignments.Find(id);
            ViewBag.TaskName = assignment.Name;
            var users = from cat in db.Users select cat;
            ViewBag.Users = users;

            return View();
        }


        [Authorize(Roles = "Organizer,Administrator")]

        [HttpPut]
        public ActionResult AssignTask(int assignmentId, string userId)
        {
            try
            {
                Assignment assignment = db.Assignments.Find(assignmentId);
                if (TryUpdateModel(assignment))
                {
                    assignment.AssigneeId = userId;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

    }
}