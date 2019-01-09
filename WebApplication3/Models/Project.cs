using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public string OrganizerId { get; set; }

        public virtual ApplicationUser Organizer { get; set; }

        public virtual ICollection<Assignment> Assignments { get; set; }

    }
}