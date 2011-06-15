using System;

namespace Events
{
	[Serializable]
	public class SentInvite : EventBase
	{
		public Guid InviteRequest;
		public DateTime SentAt;

		//TODO: Integrate this with SImone's work
//    	public Person Invitor;

		public override string ToString()
		{
			var sb = base.StringBuilder();
			return sb.ToString();
		}
	}
}