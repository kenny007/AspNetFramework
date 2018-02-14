using OONLFWK.Domain;
using OONLFWK.FIlters;
using System.ComponentModel.DataAnnotations;
using System.Web.WebPages.Html;

namespace OONLFWK.Models.Issue
{
    public class NewIssueForm 
    {
        //: IHaveIssueTypeSelectList, IHaveUserSelectList	
        [Required]
        public string Subject { get; set; }
        //[Required, DataType(DataType.MultilineText)] Our MMD Filter handles this
        
         [Required]
        public IssueType IssueType { get; set; }
       // public SelectListItem[] AvailableIssueTypes { get; set; }

        [Display(Name = "Assigned To")]
        public string AssignedToUserID { get; set; }
       // public SelectListItem[] AvailableUsers { get; set; }
        public string Body { get; set; }
	}
}