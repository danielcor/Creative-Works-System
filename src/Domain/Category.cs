using System;
using System.Collections.Generic;
using System.Linq;
using Events;

namespace Domain
{
	public class Category : Thing
	{
		public string Name { get; set; }

		// TODO: Implement this as a cached item.
		// This is a weighted Union of Tags within the Items within the database...
		// It can be eventually consistent with the tags in the database, so it doesnt have to be updated
		// in realtime, or via a query.   Though, a query might be the thing to do at the beginning to help us
		// figure out performance issues in the database.
		public Dictionary<Tag,int> Tags
		{
			get
			{
				//CreativeWorks.SelectMany<CreativeWork, IEnumerable<Tag>>(cw => cw.Tags.AsEnumerable()).GroupBy<Tag>(t => t.Name);
				var dict = new Dictionary<Tag, int>();
				foreach (var creativework in CreativeWorks)
				{
					foreach (var tag in creativework.Tags)
					{
						if (dict.ContainsKey(tag))
							dict[tag]++;
						else
							dict.Add(tag, 1);
					}
				}
				return dict;
			}
		}

		// Same is true for Materials
		public IEnumerable<Material> Materials { get { return RelatedObjects<Material>(Relation.VerbType.OverlapsMaterial); } }
		public IEnumerable<CreativeWork> CreativeWorks { get { return RelatedSubjects<CreativeWork>(Relation.VerbType.CategorizedAs); } }

		public void AddMaterial(Material material)
		{
			AddObject(Relation.VerbType.OverlapsMaterial, material);
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