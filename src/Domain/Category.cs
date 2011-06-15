using System;
using System.Collections.Generic;
using Events;

namespace Domain
{
	public class Category : Thing
	{
		public string Name { get; set; }
		public IEnumerable<Tag> Tags { get { return RelatedObjects<Tag>(Relation.VerbType.Includes); } }
		public IEnumerable<Material> Materials { get { return RelatedObjects<Material>(Relation.VerbType.Includes); } }
		public IEnumerable<CreativeWork> Creativeworks { get { return RelatedSubjects<CreativeWork>(Relation.VerbType.CategorizedAs); } } 

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