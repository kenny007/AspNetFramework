using Microsoft.AspNet.Identity.EntityFramework;
using OONLFWK.Domain;
using OONLFWK.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OONLFWK.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
       public ApplicationDbContext()
			: base("DefaultConnection")
		{
		}

		public DbSet<Issue> Issues { get; set; }
		public DbSet<LogAction> Logs { get; set; }
        

        //public static ApplicationDbContext Create()
        //{
        //    return new ApplicationDbContext();
        //}
    }
}