using System;
using System.Collections.Generic;
using Events;

namespace Domain
{
	public class Relation : Thing
	{
		protected Relation()
		{

		}

		internal Relation(Thing subject, VerbType verb, Thing obj)
		{
			Subject = subject;
			Verb = verb;
			Object = obj;
		}

		public Relation(Guid id) : base(id)
		{
		}

		
		public enum VerbType
		{
			Purchased,			 // Person or Organization => CreativeWork
			Authored,			 // Person or Organization => CreativeWork
			Owned,				 // Person or Organization => CreativeWork
			AlsoCreated,		 // Person or Organization => CreativeWork
			Displayed,
			TaggedAs,			// CreativeWork => Tags
			MadeOf,				// CreativeWork => Materials
			CategorizedAs,		// CreativeWork => Category
			Includes,           // MailingList => MailingListEntry
			IsAlso,				// Person => MailingListEntry
		};

		//public enum RelationshipType
		//{
		//    OneToOne,
		//    OneToMany,
		//    ManyToOne,
		//    ManyToMany,
		//    OneOrZeroToOne,
		//    OneToOneOrZero
		//};

		//private static Dictionary<VerbType, List<RelationshipType>> _relationshipTypes
		//    = new Dictionary<VerbType, List<RelationshipType>>
		//        {
		//            {VerbType.Created, new List<RelationshipType>() {RelationshipType.OneToMany}},
		//            {VerbType.Owned, new List<RelationshipType>() {RelationshipType.OneToMany}}
		//        };

	

		public VerbType Verb { get; internal set; }
		public Thing Subject { get; internal set; }
		public Thing Object { get; internal set; }


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