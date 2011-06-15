using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Events;

namespace Domain
{
	public class MailingListEntry : Thing
	{
		private enum Status
		{
			Requested,
			Removed
		};

		public string FirstName { get; private set; }
		public string LastName { get; private set; }
		public string Email { get; private set; }
		public bool HtmlEmail { get; private set; }

		public string Ip { get; set; }

		public MailingListEntry(Guid id)
			: base(id)
		{
		}

		public IEnumerable<MailingList> MailingLists()
		{
			return RelatedSubjects<MailingList>(Relation.VerbType.Includes).ToList().AsReadOnly();
		}

		public void AddedToMailingList(MailingList mailingList)
		{
			if (mailingList == null)
				throw new ArgumentNullException("mailingList");

			mailingList.Add(this);
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			const string lineTermination = "\n";

			sb.AppendFormat("Id: {0}{1}", Id.ToString(), lineTermination);
			sb.AppendFormat("Email: {0}{1}", Email, lineTermination);
			sb.AppendFormat("Received At: {0}{1}", CreatedAt, lineTermination);
			sb.AppendFormat("HtmlEmail: {0}", HtmlEmail);
			return sb.ToString();
		}

		public override void OnReceiving(EventBase e)
		{
			if (e is AddedToMailingList)
				OnAddedToMailingList(e as AddedToMailingList);
		}

		private void OnAddedToMailingList(AddedToMailingList addedToMailingListEvent)
		{
			if (addedToMailingListEvent == null)
				throw new ArgumentNullException("addedToMailingListEvent");

			FirstName = addedToMailingListEvent.FirstName;
			LastName = addedToMailingListEvent.LastName;
			Email = addedToMailingListEvent.Email;
			HtmlEmail = addedToMailingListEvent.HtmlEmail;
		}
	}
}
