using System;
using System.Collections.Generic;
using System.Linq;
using SpreadsheetData;

namespace Events
{
	[Serializable]
	public class RequestInviteReceived : EventBase
	{
		public string Email { get; set; }
		public DateTime ReceivedAt { get;  set; }
		
		public List<Guid> Categories { get;  set; }
		public string FirstName { get;  set; }
		public string LastName { get;  set; }
		public string Location { get;  set; }
		public string Referral { get;  set; }
		public string Phone { get;  set; }
		public string BestTimeToReach { get;  set; }
		public string WhyAreYouInterested { get;  set; }
		public Guid?  OnMailinglist { get; set; }

		protected RequestInviteReceived() { }
		public RequestInviteReceived(Guid id) : base(id) { }

		public override String ToString()
		{
			var categories = InitCategory.AllCategories();
			if (categories != null)
				return ToString(categories.ToDictionary(c => c.Id, c => c.Name));
			return ToString(null);
		}

		private String ToString(Dictionary<Guid, string> dictionary)
		{
			const string lineTermination = "\r\n";
			var sb = base.StringBuilder();

			sb.AppendFormat("Name: {0} {1}{2}", FirstName, LastName, lineTermination);
			sb.AppendFormat("Email: {0}{1}", Email, lineTermination);

			if (dictionary != null)
			{
				var categories = Categories.Select<Guid, String>(id => dictionary[id]);
				sb.AppendFormat("Categories: {0}{1}", categories.Aggregate((cat1, cat2) => cat1 + ", " + cat2), lineTermination);
			}
			else
			{
				// Since there is no Guid to Category transformation just list Guids.
				sb.AppendFormat("Categories: {0}{1}", Categories.Select(id => id.ToString()).Aggregate((cat1, cat2) => cat1 + ", " + cat2), lineTermination);
			}
			sb.AppendFormat("Location: {0}{1}", Location, lineTermination);
			sb.AppendFormat("Referral: {0}{1}", Referral, lineTermination);
			sb.AppendFormat("Phone: {0}{1}", Phone, lineTermination);
			sb.AppendFormat("Best Time to Reach: {0}{1}", BestTimeToReach, lineTermination);
			sb.AppendFormat("Why Are you interested: {0}{1}", WhyAreYouInterested, lineTermination);
			return sb.ToString();
		}
	}
}