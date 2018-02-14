using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using OONLFWK.Domain;
//using OONLFWK.Filters;
using OONLFWK.Infrastructure.Mapping;
using OONLFWK.FIlters;
using System.ComponentModel;

namespace OONLFWK.Models.Issue
{
    public class EditIssueForm : IMapFrom<Domain.Issue>
		//IHaveUserSelectList, IHaveIssueTypeSelectList
	{
        [HiddenInput]
		public int IssueID { get; set; }
        [ReadOnly(true)]
        public string CreatorUserName { get; set; }
        //[Required,DataType(DataType.MultilineText)] Handled by ModelMetadata Filter
		public string Subject { get; set; }
        //[Display(Name = "Issue Type"), DataType("IssueType")] Not needed again our LabelConvention Filter handles that
        public IssueType IssueType { get; set; }
        [Display(Name = "Assigned To")]
        public string AssignedToID { get; set; }
		public string Body { get; set; }
     
        //public SelectListItem[] AvailableIssueTypes { get; set; }
        //public SelectListItem[] AvailableUsers { get; set; }
	}
}