using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using SpreadsheetData;

namespace Commands
{
	public class RequestInvitationCommand : CommandBase
    {
       //  [AtLeastOneCheckbox(ErrorMessage = "At least one category must be checked")]
        [Display(Name = "Categories:")]
        public List<Guid> Categories { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(25, MinimumLength = 2)]
        [Display(Name = "First Name:")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Last Name:")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is required (to send invite)")]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Email:")]
        public string Email { get; set; }

		[Display(Name = "Join Email List")]
		public bool JoinEmailList { get; set; }

		public string Ip { get; set; }

		[Display(Name = "Can you receive HTML email?")]
		public bool HtmlEmail { get; set; }

        [Required(ErrorMessage = "General location is required, we are limiting invites by locality to build community")]
        [StringLength(50, MinimumLength = 10)]
        [Display(Name = "Location (City, State, Country)")]
        public string Location { get; set; }

        [StringLength(50)]
        [Display(Name = "Who referred you:")]
        public string Referral { get; set; }

        [StringLength(30)]
        [Display(Name = "Phone Number:")]
        public string Phone { get; set; }

        [StringLength(50)] 
        [Display(Name = "Best time to reach you:")]
        public string BestTimeToReach { get; set; }

        [StringLength(500)]
        [Display(Name = "Why are you interested in MueVue:")]
        public string WhyAreYouInterested { get; set;}
        
        //[Required(ErrorMessage = "First name is required")]
        //[StringLength(25, MinimumLength = 3)]
        //[Display(Name = "First Name:")]
        //[RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed")]
        //[ScaffoldColumn(false)]

        String ToString(Dictionary<Guid, string> dictionary)
        {
            var sb = new StringBuilder();
			const string lineTermination = "\r\n";

            sb.AppendFormat("Requesting an invitation{0}", lineTermination);
            sb.AppendFormat("Name: {0} {1}{2}", FirstName, LastName, lineTermination);
            sb.AppendFormat("Email: {0}{1}", Email, lineTermination);

			if (JoinEmailList)
			{
				sb.AppendFormat("User requested to be added to mailing list{0}", lineTermination);
				sb.AppendFormat("User email supports HTML: {0}{1}", HtmlEmail, lineTermination);
				sb.AppendFormat("User's IP address: {0}{1}", Ip, lineTermination);
			}

        	if (Categories != null)
			{
				var categories = Categories.Select<Guid, String>(id => dictionary[id]);
				sb.AppendFormat("Categories: {0}{1}", categories.Aggregate((cat1, cat2) => cat1 + ", " + cat2), lineTermination);	
			}
            sb.AppendFormat("Location: {0}{1}", Location, lineTermination);
            sb.AppendFormat("Referral: {0}{1}", Referral, lineTermination);
            sb.AppendFormat("Phone: {0}{1}", Phone,lineTermination);
            sb.AppendFormat("Best Time to Reach: {0}{1}", BestTimeToReach, lineTermination);
            sb.AppendFormat("Why Are you interested: {0}{1}", WhyAreYouInterested, lineTermination);
            return sb.ToString();
        }

		public override String ToString()
		{
			if (InitCategory.AllCategories() != null)
				return ToString(InitCategory.AllCategories().ToDictionary(c => c.Id, c => c.Name));
			return ToString(null);
		}
    }
}