using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Commands
{
	public class RequestMailingCommand : CommandBase
	{
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

		[Display(Name = "Can you receive HTML email?")]
		public bool HtmlEmail { get; set;}

		public string Ip { get; set; }

		//[Required(ErrorMessage = "First name is required")]
		//[StringLength(25, MinimumLength = 3)]
		//[Display(Name = "First Name:")]
		//[RegularExpression(@"(\S)+", ErrorMessage = "White space is not allowed")]
		//[ScaffoldColumn(false)]
		public override string ToString()
		{
			var sb = new StringBuilder();
			const string lineTermination = "\r\n";

			sb.AppendFormat("FirstName: {0}", FirstName);
			sb.AppendFormat("LastName: {0}", LastName);
			sb.AppendFormat("Email: {0}{1}", Email, lineTermination);
			sb.AppendFormat("HtmlEmail: {0}", HtmlEmail);
			return sb.ToString();
		}
	}
}