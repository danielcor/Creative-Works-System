using System;
using System.Collections.Generic;
using System.Linq;
using Events;

namespace Domain
{
	public abstract class Thing : IEquatable<Thing>
	{
		protected Thing()  {}

		protected Thing(Guid id)
		{
			Id = id;
			CreatedAt = DateTime.UtcNow;
		}

		public Guid Id { get; private set; }
		public DateTime CreatedAt { get; protected set; }

		//TODO: Add whatever we need to manage multiple updaters to Thing.Relations
		protected IList<Relation> Relations { get; private set; }
		public IEnumerable<Relation> PublicRelations { get { return (Relations ?? new List<Relation>()); } }

		protected Thing RelatedSubject(Relation.VerbType verb)
		{
			if (Relations == null)
				return default(Thing);

			var relation = Relations.FirstOrDefault(r => r.Verb == verb && r.Object == this);
			return (relation == null) ? default(Thing) : relation.Subject;
		}

		protected Thing RelatedSubject(IEnumerable<Relation.VerbType> verbs)
		{
			if (Relations == null)
				return default(Thing);

			var relation = Relations.FirstOrDefault(r => verbs.Any(v => v == r.Verb) && r.Object == this);
			return (relation == null) ? default(Thing) : relation.Subject;
		}

		protected IEnumerable<Thing> RelatedSubjects(Relation.VerbType verb)
		{
			return Relations == null
				? new List<Thing>() 
				: Relations.Where(r => r.Verb == verb && r.Object == this).Select(r =>r.Subject);
		}

		protected IEnumerable<Thing> RelatedSubjects(IEnumerable<Relation.VerbType> verbs)
		{
			return Relations == null
				? new List<Thing>()
				: Relations.Where(r => verbs.Any(v => v == r.Verb) && r.Object == this).Select(r => r.Subject);
		}

		protected T RelatedSubject<T>(Relation.VerbType verb) where T: Thing
		{
			if (Relations == null)
				return default(T);

			return Relations.Where(r => r.Verb == verb && r.Object == this && r.Subject is T)
							.Select(r => r.Subject as T)
							.FirstOrDefault();
		}

		protected T RelatedSubject<T>(IEnumerable<Relation.VerbType> verbs) where T:Thing
		{
			if (Relations == null)
				return default(T);

			return Relations.Where(r => verbs.Any(v => v == r.Verb) && r.Object == this && r.Subject is T)
							.Select(r => r.Subject as T)
							.FirstOrDefault();
		}

		protected IEnumerable<T> RelatedSubjects<T>(Relation.VerbType verb) where T: Thing
		{
			return Relations == null
				? new List<T>()
			    : Relations.Where(r => r.Verb == verb && r.Object == this && r.Subject is T).Select(r => r.Object as T);
		}

		protected IEnumerable<T> RelatedSubjects<T>(IEnumerable<Relation.VerbType> verbs) where T : Thing
		{
			return Relations == null
				? new List<T>()
				: Relations.Where(r => verbs.Any(v => v == r.Verb) && r.Object == this && r.Subject is T).Select(r => r.Object as T);
		}


		protected Thing RelatedObject(Relation.VerbType verb)
		{
			if (Relations == null)
				return default(Thing);

			var relation = Relations.FirstOrDefault(r => r.Verb == verb && r.Subject == this);
			return (relation == null) ? default(Thing) : relation.Object;
		}

		protected Thing RelatedObject(IEnumerable<Relation.VerbType> verbs)
		{
			if (Relations == null || verbs == null)
				return default(Thing);

			var relation = Relations.FirstOrDefault(r => verbs.Any(v => v == r.Verb) && r.Subject == this);
			return (relation == null) ? default(Thing) : relation.Object;
		}

		protected IEnumerable<Thing> RelatedObjects(Relation.VerbType verb)
		{
			return Relations == null
				? new List<Thing>()
				: Relations.Where(r => r.Verb == verb && r.Subject == this).Select(r => r.Object);
		}

		protected IEnumerable<Thing> RelatedObjects(IEnumerable<Relation.VerbType> verbs)
		{
			return Relations == null || verbs == null
				? new List<Thing>()
				: Relations.Where(r => verbs.Any(v => v == r.Verb) && r.Subject == this).Select(r => r.Object);
		}

		protected T RelatedObject<T>(Relation.VerbType verb) where T : Thing
		{
			if (Relations == null)
				return default(T);
			return Relations.Where(r => r.Verb == verb && r.Subject == this && r.Object is T)
							.Select(r => r.Object as T)
							.FirstOrDefault();
		}

		protected T RelatedObject<T>(IEnumerable<Relation.VerbType> verbs) where T : Thing
		{
			if (Relations == null || verbs == null)
				return default(T);

			return Relations.Where(r => verbs.Any(v => v == r.Verb) && r.Subject == this && r.Object is T)
							.Select(r => r.Object as T)
							.FirstOrDefault();
		}

		protected IEnumerable<T> RelatedObjects<T>(Relation.VerbType verb) where T : Thing
		{
			return Relations == null
				? default(IEnumerable<T>)
				: Relations.Where(r => r.Verb == verb && r.Subject == this && r.Object is T).Select(r => r.Object as T);
		}

		protected IEnumerable<T> RelatedObjects<T>(IEnumerable<Relation.VerbType> verbs) where T : Thing
		{
			return Relations == null
				? default(IEnumerable<T>)
				: Relations.Where(r => verbs.Any(v => v == r.Verb) && r.Subject == this && r.Object is T).Select(r => r.Object as T);
		}

		// This function should really only be caled if the type of Other doesn't equal this
		// Derived classes should reimplement
		public virtual bool Equals(Thing other)
		{
			if (other == this) return true;
			if (GetType() != other.GetType())
				return false;
 
			return (Id == other.Id);
		}

		public override abstract String ToString();
		public abstract void OnReceiving(EventBase e);

		protected void AddSubject(Relation.VerbType verb, Thing subject)
		{
			if (Relations == null) Relations = new List<Relation>();
			var relation = new Relation(subject, verb, this);
			Relations.Add(relation);
			subject.Add(relation);
		}

		// IEquatable & IEquatableComparer implemented for Relation should make .Contains work.
		private bool Add(Relation relation)
		{
			if (Relations == null) Relations = new List<Relation>();

			// TODO: Should this return false?  or should it throw an exception.
			// To some extent this is a failure, and shouldn't happen
			if (Relations.Contains(relation)) return false;
			Relations.Add(relation);
			return true;
		}

		protected void AddObject(Relation.VerbType verb, Thing obj)
		{
			if (Relations == null) Relations = new List<Relation>();
			var relation = new Relation(this, verb, obj);

			// TODO: what do we do if either add fails?
			// TODO: handle multi-threading
			// This will be wrapped in a transaction, and generally we will need to roll it back.
			Relations.Add(relation);
			obj.Add(relation);
		}

		private void Remove(Relation relation)
		{
			if (Relations == null) return;
			if (!Relations.Contains(relation)) return;

			// TODO: we have to actually remove it from the database, not just remove all references
			Relations.Remove(relation);
		}

		protected void RemoveSubject(Relation.VerbType verb, Thing subject)
		{
			if (Relations == null) return;
			var relation = FindRelation(subject, verb, this);

			if (relation == null) return;
			Remove(relation);
		}

		protected void RemoveObject(Relation.VerbType verb, Thing obj)
		{
			if (Relations == null) return;
			var relation = FindRelation(this, verb, obj);

			if (relation == null) return;
			Remove(relation);
		}

		private Relation FindRelation(Thing subject, Relation.VerbType verb, Thing obj)
		{
			return Relations == null
				? null
				: Relations.FirstOrDefault(r => r.Verb == verb && r.Subject == subject && r.Object == obj);
		}
	}
}
