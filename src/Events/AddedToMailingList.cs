using System;

namespace Events
{
    [Serializable]
    public class AddedToMailingList : EventBase
    {
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
		public bool HtmlEmail { get; set; }

		public Guid MailingListId { get; set; }

    	public override string ToString()
		{
			const string lineTermination = "\r\n";
    		var sb = base.StringBuilder()
    			.AppendFormat("Email: {0}{1}", Email, lineTermination)
				.AppendFormat("FirstName: {0}{1}", FirstName, lineTermination)
				.AppendFormat("LastName: {0}{1}", LastName, lineTermination)
				.AppendFormat("HtmlEmail: {0}{1}", HtmlEmail, lineTermination);
    		return sb.ToString();
    	}
    }
}