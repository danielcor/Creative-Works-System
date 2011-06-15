using System;
using System.Collections.Generic;
using System.Linq;
using Events;

namespace Domain
{
	public class CreativeWork : Thing
	{
		public IEnumerable<Tag> Tags { get { return base.RelatedObjects<Tag>(Relation.VerbType.TaggedAs); } }
		public IEnumerable<Category> Categories { get { return RelatedObjects<Category>(Relation.VerbType.CategorizedAs); } }
		public IEnumerable<Material> Materials { get { return RelatedObjects<Material>(Relation.VerbType.MadeOf); } }

		public DateTime DateCreated { get; set; }
		public string Title { get; set; }

		public Thing Author
		{
			get { return RelatedSubject<CreativeWork>(Relation.VerbType.Authored); }
			set
			{
				if (Author != null)
					throw new ArgumentException("There already exists an (primary) author for this creative work");
				AddSubject(Relation.VerbType.Authored, value);
			}
		}

		public Person AuthorPerson { get { return Author as Person; } }
//		public Organization AuthorOrganization { get { return Author as Organization; } }

		public override string ToString()
		{
			throw new NotImplementedException();
		}

		public override void OnReceiving(EventBase e)
		{
			throw new NotImplementedException();
			//switch(e)
			//{
			//    //case AuthorUploadedCreativeWork:
			//    //case PatronUploadedCreativeWork:
			//    //case AuthorModifiedCreativeWork:
			//    //case PatronModifiedCreativeWork:
			//    //case CreativeWorkRated:
			//    //case CreativeWorkCritqued:
			//    //case CreativeWorkRented:
			//    //case CreativeWorkSold:
			//    //default:
			//}
		}
	}
}