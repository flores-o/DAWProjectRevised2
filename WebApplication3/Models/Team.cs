using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Team
    {
        [Key]
        public int TeamId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Members { get; set; }

    }
}