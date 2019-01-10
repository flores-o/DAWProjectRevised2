using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;
using Microsoft.AspNet.Identity;

namespace WebApplication3.Controllers
{
    public class CommentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comment
        public async Task<ActionResult> Index()
        {
            var comments = db.Comments.Include(c => c.Assignment).Include(c => c.Author);
            return View(await comments.ToListAsync());
        }

        // GET: Comment/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comment/Create
        public ActionResult Create()
        {
            ViewBag.AssignmentId = new SelectList(db.Assignments, "AssignmentId", "Name");
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: Comment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CommentId,Text,AuthorId,AssignmentId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AssignmentId = new SelectList(db.Assignments, "AssignmentId", "Name", comment.AssignmentId);
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "Name", comment.AuthorId);
            return View(comment);
        }

        // GET: Comment/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);
            if (comment.AuthorId == User.Identity.GetUserId() ||
                   User.IsInRole("Administrator"))
            {
                if (comment == null)
                {
                    return HttpNotFound();
                }
                ViewBag.AssignmentId = new SelectList(db.Assignments, "AssignmentId", "Name", comment.AssignmentId);
                ViewBag.AuthorId = new SelectList(db.Users, "Id", "Name", comment.AuthorId);
                return View(comment);
            } else
            {
                return Redirect("/Assignment");
            }
        }

        // POST: Comment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CommentId,Text,AuthorId,AssignmentId")] Comment comment)
        {
            if (comment.AuthorId == User.Identity.GetUserId() ||
                   User.IsInRole("Administrator"))
            {

                if (ModelState.IsValid)
                {
                    db.Entry(comment).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    //return RedirectToAction("Index");
                    return Redirect("/Assignment");

                }
                ViewBag.AssignmentId = new SelectList(db.Assignments, "AssignmentId", "Name", comment.AssignmentId);
                ViewBag.AuthorId = new SelectList(db.Users, "Id", "Name", comment.AuthorId);
                return View(comment);
            } else
            {
                return Redirect("/Assignment");
            }
        }

        // GET: Comment/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = await db.Comments.FindAsync(id);

            if (comment.AuthorId == User.Identity.GetUserId() ||
                   User.IsInRole("Administrator"))
            {
                if (comment == null)
                {
                    return HttpNotFound();
                }
                return View(comment);
            }
            else
            {
                return Redirect("/Assignment");
            }
        
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Comment comment = await db.Comments.FindAsync(id);
            db.Comments.Remove(comment);
            await db.SaveChangesAsync();
            return Redirect("/Assignment");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
