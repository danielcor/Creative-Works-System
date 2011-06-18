using System;
using System.Collections.Generic;
using System.Text;
using Events;
using System.Linq;

namespace Domain
{
	public class MailingList : Thing 
	{
		public string ListName { get; set;  }
		public String ListId { get; set; }

		public MailingList InterestedList
		{
			get
			{
				var m = new MailingList(new Guid("3951A9D4-E5C8-4226-A81D-5483D850DF73"))
							{
								ListId = "cca880b05f",
								ListName = "Interested List",
								CreatedAt = new DateTime(2011,6,1)
							};
				return m;
			}
		}

		public IEnumerable<MailingListEntry> MailingListEntries { get { return base.RelatedObjects<MailingListEntry>(Relation.VerbType.Includes); } }


		internal void Add(MailingListEntry mailingListEntry)
		{
			AddObject(Relation.VerbType.Includes, mailingListEntry);
		}

		public MailingList(Guid id) : base(id)
		{
		}

		public override string ToString()
		{
			throw new NotImplementedException();
		}

		public override void OnReceiving(EventBase e)
		{
			throw new NotImplementedException();
		}

	}
}