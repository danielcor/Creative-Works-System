using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Events;

namespace Domain
{
	public abstract class Thing
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
				? default(IEnumerable<T>)
				: Relations.Where(r => r.Verb == verb && r.Object == this && r.Subject is T).Select(r => r.Object as T);
		}

		protected IEnumerable<T> RelatedSubjects<T>(IEnumerable<Relation.VerbType> verbs) where T : Thing
		{
			return Relations == null
				? default(IEnumerable<T>)
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
			if (Relations == null)
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
			return Relations == null
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
			if (Relations == null)
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

		public override abstract String ToString();
		public abstract void OnReceiving(EventBase e);

		protected void AddSubject(Relation.VerbType verb, Thing subject)
		{
			if (Relations == null) Relations = new List<Relation>();

		}

		protected internal void Add(Relation relation)
		{

		}

		protected void AddObject(Relation.VerbType verb, Thing obj)
		{

		}

		internal void Remove(Relation relation)
		{

		}
	}
}
