using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using Events;
using System.Linq;
using SpreadsheetData;

namespace Domain
{
    public class Invitation : Thing
    {
        private enum Status
        {
            Requested,
            Invited,
            Rejected,
            Waitlist
        };

        // private readonly Guid InvitationARId = new Guid("DA26A776-57C5-439D-9B8D-58DED13B0575");
        private string _email;
        private DateTime _receivedAt;
        private Status _status;

        private List<Guid> _categories;

        private string _firstName;
        private string _lastName;
        private string _location;
        private string _referral;
        private string _phone;
        private string _bestTimeToReach;
        private string _whyAreYouInterested;

        public Invitation(Guid id) : base(id)
        {
        }

        public override string ToString()
        {   
            var sb = new StringBuilder();
            const string lineTermination = "\n";

            var categories = _categories == null ? null : _categories.Select(id => InitCategory.IdDictionary[id].Name);

            sb.AppendFormat("Id: {0}{1}", Id.ToString(), lineTermination);
            sb.AppendFormat("Email: {0}{1}", _email, lineTermination);
            sb.AppendFormat("Received At: {0}{1}", _receivedAt, lineTermination);
            sb.AppendFormat("Categories: {0}{1}", categories == null ? "none" : categories.Aggregate((cat1, cat2) => cat1 + ", " + cat2), lineTermination);
            sb.AppendFormat("FirstName: {0}{1}", _firstName, lineTermination);
            sb.AppendFormat("LastName: {0}{1}", _lastName, lineTermination);
            sb.AppendFormat("Location: {0}{1}", _location, lineTermination);
            sb.AppendFormat("Referral: {0}{1}", _referral, lineTermination);
            sb.AppendFormat("Phone: {0}{1}", _phone, lineTermination);
            sb.AppendFormat("Best Time to Reach: {0}{1}", _bestTimeToReach, lineTermination);
            sb.AppendFormat("Why Are you interested: {0}{1}", _whyAreYouInterested, lineTermination);
            return sb.ToString();
        }

		public override void OnReceiving(EventBase e)
		{
			if (e is RequestInviteReceived)
				OnReceiving(e as RequestInviteReceived);
			else
				throw new NotImplementedException();
		}

        public void OnReceiving(RequestInviteReceived e)
        {
			if  (null == e)
				throw new ArgumentNullException();

			_email = e.Email;
            _receivedAt = e.ReceivedAt;
            _status = Status.Requested;
            _categories = e.Categories;
            _firstName = e.FirstName;
            _lastName = e.LastName;
            _location = e.Location;
            _referral = e.Referral;
            _phone = e.Phone;
            _bestTimeToReach = e.BestTimeToReach;
            _whyAreYouInterested = e.WhyAreYouInterested;
        }

		public void OnReceivingRequestInvite(RequestInviteReceived e)
			{
			throw new NotImplementedException();
			}
	}
}