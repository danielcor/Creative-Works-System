using System;
using System.Collections.Generic;
using System.Linq;
using Events;

namespace Domain
{
	public class Person : Thing
	{
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