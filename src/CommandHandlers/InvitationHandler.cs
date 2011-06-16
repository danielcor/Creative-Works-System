using System;
using Commands;
using Domain;
using Events;
//using Infrastructure.Config;
//using Infrastructure.Email;

namespace CommandHandlers
{
	//TODO: Build abstract database class, along with Versant version of it.
	//TODO: Build Versant implementation of abstract  database class
	//TODO: Build Alternative implementation of abstract database class.
	public class Database
	{

	};

	public class InvitationHandler
	{
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

		private Database Database { get; set; }

		public InvitationHandler() { }

	// TODO: Add  Database code here...
		public bool Process(RequestInvitationCommand command)
		{
			bool inviteSuccess = false;
			bool mailSuccess = false;
			bool mailingListSuccess = false;

			var eventGuid = Guid.NewGuid();

			Logger.Debug("Received RequestMailing Commmand: {0}", command);

			try
			{
				var invitation = new Invitation(Guid.NewGuid());

				var e = new RequestInviteReceived(eventGuid)
						{
							StreamId = invitation.Id,
							Email = command.Email,
							ReceivedAt = DateTime.UtcNow,
							Categories = command.Categories,
							FirstName = command.FirstName,
							LastName = command.LastName,
							Location = command.Location,
							Referral = command.Referral,
							Phone = command.Phone,
							BestTimeToReach = command.BestTimeToReach,
							WhyAreYouInterested = command.WhyAreYouInterested
						};

				//var configurationConnectionFactory = new ConfigurationConnectionFactory(MueVueConfig.MueVueEventsName);
				//var connection = configurationConnectionFactory.OpenMaster(e.StreamId);

				MueVueEvents.Add(e);
				invitation.OnReceiving(e);
				inviteSuccess = true;
			}
			catch (Exception ex)
			{
				Logger.Error("Saving invitationRequest to event store failed: ", ex);
			}


			if (command.JoinEmailList)
			{
				var requestMailingListCommand = new RequestMailingCommand
				                                	{
														FirstName = command.FirstName,
														LastName = command.LastName,
														HtmlEmail = command.HtmlEmail,
														Ip = command.Ip
				                                	};
				mailingListSuccess = CommandHandler.Process(requestMailingListCommand);
			}

			try
			{
				Logger.Debug("Calling SmtpMail.send()");
				//SmtpMail.Send("<xxx>", "Received Invitation Request", command.ToString(), command.Email,
				//    null, null, true, eventGuid, null);
				mailSuccess = true;  
			}
			catch (Exception ex)
			{
				Logger.ErrorException("Calling SmtpMail.send() failed", ex);
				Logger.Error("Failed to send email", ex);
			}

			var success = inviteSuccess || mailSuccess;

			Logger.Info("RequestInvitation - eventstore: {0}, mailing: {1}, mailingList: {2}", inviteSuccess, mailSuccess, mailingListSuccess);
			Logger.Info("Request: {0}", command);
			return success;
		}
	}
}
