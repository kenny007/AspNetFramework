﻿using System.Web.Mvc;
using Microsoft.Web.Mvc;
using OONLFWK.Data;
using OONLFWK.Domain;
using OONLFWK.FIlters;
using OONLFWK.Infrastructure;
using OONLFWK.Models;
using OONLFWK.Models.Issue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper.QueryableExtensions;
using OONLFWK.Infrastructure.Alerts;
using System.Collections;

namespace OONLFWK.Infrastructure
{
    public class IssueController : IssueLogController
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUser _currentUser;

        public IssueController(ApplicationDbContext context,
            ICurrentUser currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }


        [ChildActionOnly]
        public ActionResult YourIssuesWidget()
        {
            var models = _context.Issues.Where(i => i.AssignedTo.Id == _currentUser.User.Id)
                .Project().To<IssueSummaryViewModel>();
            return PartialView(models.ToArray());
        }

        [ChildActionOnly]
        public ActionResult CreatedByYouWidget()
        {
            var models = _context.Issues.Where(i => i.Creator.Id == _currentUser.User.Id)
                .Project().To<IssueSummaryViewModel>();

            return PartialView(models.ToArray());
        }

        [ChildActionOnly]
        public ActionResult AssignmentStatsWidget()
        {
            var stats = _context.Users.Project().To<AssignmentStatsViewModel>();

            return PartialView(stats.ToArray());
        }

       // [ChildActionOnly]
        public ActionResult IssueWidget()
        {
            var models = from i in _context.Issues
                         select new IssueSummaryViewModel
                         {
                             IssueID = i.IssueID,
                             Subject = i.Subject,
                             CreatedAt = i.CreatedAt
                         };

            return PartialView(models.ToArray());
        }

        public ActionResult New()
        {
            var form = new NewIssueForm();
            return View(form);
        }

        [HttpPost, ValidateAntiForgeryToken, Log("Created issue")]
        public ActionResult New(NewIssueForm form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var assignedToUser = _context.Users.Single(u => u.Id == form.AssignedToUserID);

            _context.Issues.Add(new Issue(_currentUser.User, assignedToUser, form.IssueType, form.Subject, form.Body));

            _context.SaveChanges();

            return RedirectToAction<HomeController>(c => c.Index())
                .WithSuccess("Issue created!");
        }

        [Log("Viewed issue {id}")]
        public ActionResult View(int id)
        {
            var model = _context.Issues
                .Project().To<IssueDetailsViewModel>()
                .SingleOrDefault(i => i.IssueID == id);

            if (model == null)
            {
               // throw new  ApplicationException("Issue Not Found or Deleted");
                return RedirectToAction<HomeController>(c => c.Index())
                    .WithError("Unable to find the issue.  Maybe it was deleted?");
            }
            return View(model);
        }

        [Log("Started to edit issue {id}")]
        public ActionResult Edit(int id)
        {
            var form = _context.Issues
                .Project().To<EditIssueForm>()
                .SingleOrDefault(i => i.IssueID == id);

            if (form == null)
            {
                return RedirectToAction<HomeController>(c => c.Index())
                    .WithError("Unable to find the issue.  Maybe it was deleted?");
            }

            return View(form);
        }

        [HttpPost, Log("Saving changes")]
        public ActionResult Edit(EditIssueForm form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var issue = _context.Issues.SingleOrDefault(i => i.IssueID == form.IssueID);

            if (issue == null)
            {
                return RedirectToAction<HomeController>(c => c.Index())
                    .WithError("Unable to find the issue.  Maybe it was deleted?");
            }

            var assignedToUser = _context.Users.Single(u => u.Id == form.AssignedToID);

            issue.Subject = form.Subject;
            issue.AssignedTo = assignedToUser;
            issue.Body = form.Body;
            issue.IssueType = form.IssueType;

            return this.RedirectToAction(c => c.View(form.IssueID))
                .WithSuccess("Changes saved!");
        }

        [HttpPost, ValidateAntiForgeryToken, Log("Deleted issue {id}")]
        public ActionResult Delete(int id)
        {
            var issue = _context.Issues.Find(id);

            if (issue == null)
            {
                throw new ApplicationException("Issue not found!");
            }

            _context.Issues.Remove(issue);

            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

    }
}