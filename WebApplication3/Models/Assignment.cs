using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Assignment
    {
        [Key]
        public int AssignmentId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public int Status { get; set; }


        public string StartDate { get; set; }
        public string EndDate { get; set; }

        /*public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }*/

        public int ProjectId { get; set; }

        public string AssigneeId { get; set; }

        public virtual ApplicationUser Assignee { get; set; }
        public virtual Project Project { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }


    }

    /*
    public class AssignmentDBContext : DbContext
    {
        public AssignmentDBContext() : base("DBConnectionString") { }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ApplicationUser> Assignees { get; set; }

    }*/



}