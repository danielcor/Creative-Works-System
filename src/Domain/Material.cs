using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Events;

namespace Domain
{

	public class Material : Thing
	{
		public string Name { get; set; }
		public IEnumerable<Category> Categories { get { return base.RelatedSubjects<Category>(Relation.VerbType.OverlapsMaterial); } }
		public IEnumerable<CreativeWork> CreativeWorks { get { return base.RelatedSubjects<CreativeWork>(Relation.VerbType.MadeOf); } }

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
