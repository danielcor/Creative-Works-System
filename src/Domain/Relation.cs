using System;
using System.Collections.Generic;
using Events;

namespace Domain
{
	// Relation is really *NOT* a Thing...   
	public class Relation : IEquatable<Relation>, IComparable<Relation>
	{
		public DateTime CreatedAt { get; private set; }
		public VerbType Verb { get; internal set; }
		public Thing Subject { get; internal set; }
		public Thing Object { get; internal set; }
		public int Value { get; internal set; }    // Depending on the Verb this can be a count or a ranking

		public bool Equals(Relation other)
		{
			if (other == this)
				return true;

			if (other == null || other.GetHashCode() != GetHashCode())
				return false;

			return Subject.Equals(other.Subject) && Verb.Equals(other.Verb) && Object.Equals(other.Object);
		}

		internal Relation()
		{
			CreatedAt = DateTime.UtcNow;
		}

		internal Relation(Thing subject, VerbType verb, Thing obj) : this(subject, verb, 1, obj)
		{		
		}

		internal Relation(Thing subject, VerbType verb, int value, Thing obj)
		{
			Subject = subject;
			Verb = verb;
			Value = value;
			Object = obj;

			CreatedAt = DateTime.UtcNow;
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
			OverlapsTag,        // Category => Tag
			OverlapsMaterial,   // Category => Material
			Includes,           // MailingList => MailingListEntry
			IsAlso,				// Person => MailingListEntry
		};

		public int IncrementValue(int increment)
		{
			return (Value += increment);
		}

		public override int GetHashCode()
		{
			int hCode = Subject.GetHashCode() + Verb.GetHashCode() + Object.GetHashCode();
			return hCode.GetHashCode();
		}

		// This allows us to create an ordered list of Authors for example, where the 1st author is the primary one.
		public int CompareTo(Relation other)
		{
			if (other == null) return 1;
			return Value.CompareTo(other.Value);
		}

		public override string ToString()
		{
			throw new NotImplementedException();
		}

		public virtual void OnReceiving(EventBase e)
		{
			throw new NotImplementedException();
		}
	}

	class RelationEqualityComparer : IEqualityComparer<Relation>
	{
		public bool Equals(Relation r1, Relation r2)
		{
			if (r1 == r2)
				return true;

			if (r1 == null || r2 == null)
				return false;

			return r1.Subject.Equals(r2.Subject) && r1.Verb.Equals(r2.Verb) && r1.Object.Equals(r2.Object);
		}

		public int GetHashCode(Relation r)
		{
			int hCode = r.Subject.GetHashCode() + r.Verb.GetHashCode() + r.Object.GetHashCode();
			return hCode.GetHashCode();
		}
	}
}