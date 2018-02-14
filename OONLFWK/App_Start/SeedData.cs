using OONLFWK.Data;
using OONLFWK.Domain;
using OONLFWK.Infrastructure.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OONLFWK.App_Start
{
    public class SeedData : IRunAtStartup
    {
        private readonly ApplicationDbContext _context;

        public SeedData(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Execute()
        {
            var user1 = _context.Users.FirstOrDefault() ??
                        _context.Users.Add(new ApplicationUser { UserName = "Kennedy" });

            var user2 = _context.Users.FirstOrDefault(u => u.UserName == "Samson") ??
                        _context.Users.Add(new ApplicationUser { UserName = "Samson" });

            var user3 = _context.Users.FirstOrDefault(u => u.UserName == "Adebayor") ??
                        _context.Users.Add(new ApplicationUser { UserName = "Adebayor" });

            _context.SaveChanges();

            if (!_context.Issues.Any())
            {
                _context.Issues.Add(new Issue(user2, user1, IssueType.Bug, "Viewing details crashes", "Sometimes, viewing an issue's details will cause a crash.  It seems to only happen when there is a full moon out!"));
                _context.Issues.Add(new Issue(user3, user1, IssueType.Support, "Second account", "I need a second account for my cat to use.  My cat finds all kinds of bugs, and I really want him to be able to log the issues himself."));
                _context.Issues.Add(new Issue(user1, user2, IssueType.Enhancement, "Tablet-Friendly UX", "I'd like to see the app support use from a tablet.  The web app works from a tablet, but it's clunky.  I want the UX to be streamlined and optimized for touch."));

                _context.SaveChanges();
            }
        }
    }
}