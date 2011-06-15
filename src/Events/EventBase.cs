using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace Events
{
	[ContractClass(typeof(EventBaseCodeContract))]
	public abstract class EventBase
	{
		protected EventBase() { }
		protected EventBase(Guid id)
		{
			Contract.Requires(id != Guid.Empty);
			Id = id;
		}

		public Guid Id { get; private set; }
		public Guid StreamId { get; set; }
		public DateTime StoredAt { get; set; }

		protected StringBuilder StringBuilder()
		{
			var sb = new StringBuilder();
			sb.AppendFormat("{0}:", GetType().Name);
			sb.AppendFormat("Id: {0}", StreamId);
			sb.AppendFormat("CreatedAt: {0}", StoredAt);
			return sb;
		}

		public override abstract String ToString();
	}

	[System.Diagnostics.Contracts.ContractClassFor(typeof(EventBase))]
	abstract class EventBaseCodeContract : Events.EventBase
	{
		protected new StringBuilder StringBuilder()
		{
			Contract.Ensures(null != Contract.Result<StringBuilder>());
			return default(StringBuilder);
		}

		public override string ToString()
		{
			return default(string);
		}
	}
}