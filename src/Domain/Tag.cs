using System;
using System.Collections.Generic;
using Events;

namespace Domain
{
	public class Tag : Thing
	{
		public string Name { get; set; }
		public IEnumerable<Category> Categories { get; internal set; }

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