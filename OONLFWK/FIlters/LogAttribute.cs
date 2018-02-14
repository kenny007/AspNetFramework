using OONLFWK.Data;
using OONLFWK.Domain;
using OONLFWK.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OONLFWK.FIlters
{
    //This runs after the action is executed
    public class LogAttribute : ActionFilterAttribute
    {
        //Our dependencies here are declared as public properties so that we can have setter injection done since attribute
        //don't allow constructor Injection
        private IDictionary<string, object> _parameters;
        public ApplicationDbContext Context { get; set; }
        public ICurrentUser CurrentUser { get; set; }

        public string Description { get; set; }

        public LogAttribute(string description)
        {
            Description = description;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _parameters = filterContext.ActionParameters;
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var description = Description;

            foreach (var kvp in _parameters)
            {
                description = description.Replace("{" + kvp.Key + "}", kvp.Value.ToString());
            }

            Context.Logs.Add(new LogAction(CurrentUser.User, filterContext.ActionDescriptor.ActionName,
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, description));

            Context.SaveChanges();
        }
    }
}