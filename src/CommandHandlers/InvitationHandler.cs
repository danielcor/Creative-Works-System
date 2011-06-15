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
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		private Database Database { get; set; }

		public InvitationHandler() { }

	// TODO: Add  Database code here...
		public bool Process(RequestInvitationCommand command)
		{
			bool inviteSuccess = false;
			bool mailSuccess = false;
			bool mailingListSuccess = false;

			var eventGuid = Guid.NewGuid();

			logger.Debug("Received RequestMailing Commmand: {0}", command);

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
				logger.Error("Saving invitationRequest to event store failed: ", ex);
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
				logger.Debug("Calling SmtpMail.send()");
				//SmtpMail.Send("<xxx>", "Received Invitation Request", command.ToString(), command.Email,
				//    null, null, true, eventGuid, null);
				mailSuccess = true;  
			}
			catch (Exception ex)
			{
				logger.ErrorException("Calling SmtpMail.send() failed", ex);
				logger.Error("Failed to send email", ex);
			}

			var success = inviteSuccess || mailSuccess;

			logger.Info("RequestInvitation - eventstore: {0}, mailing: {1}, mailingList: {2}", inviteSuccess, mailSuccess, mailingListSuccess);
			logger.Info("Request: {0}", command);
			return success;
		}
	}
}
