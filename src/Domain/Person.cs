using System;
using System.Collections.Generic;
using System.Linq;
using Events;

namespace Domain
{
	public class AddressEntry
	{
		public string LocationName { get; set; }
		public string Street1 { get; set; }
		public string Street2 { get; set; }
		public string State { get; set; }
		public string City { get; set; }
		public int ZipCode { get; set; }
		// TODO: Add GeoLocation information
	}

	public class EmailEntry
	{
		public string EmailName { get; set; }
		public string Email { get; set; }
		public bool HtmlEmail { get; set; }
	}

	public class PhoneEntry
	{
		public string PhoneName { get; set; }
		public string Phone { get; set; }
		public string Extension { get; set; }
	}

	public class Person : Thing
	{
		public string UserName { get; private set; }
		public string FirstName { get; private set; }
		public string LastName { get; private set; } 

		public IEnumerable<AddressEntry> Locations { get; private set; }
		public IEnumerable<EmailEntry> Emails { get; private set; }
		public IEnumerable<PhoneEntry> Phones { get; private set; }

		public IEnumerable<CreativeWork> CreativeWorksAuthored
		{
			get
			{
				return (from r in Relations where r.Subject == this && r.Verb == Relation.VerbType.Authored select r.Object as CreativeWork).ToList();
			}
		}

		public IEnumerable<CreativeWork> CreativeWorksOwned
		{
			get
			{
				return (from r in Relations where r.Subject == this && r.Verb == Relation.VerbType.Owned select r.Object as CreativeWork).ToList();
			}
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