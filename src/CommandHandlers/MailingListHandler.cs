using System;
using System.Collections.Generic;
using Commands;
using Domain;
using Events;
//using Infrastructure.Email;
using PerceptiveMCAPI;
using PerceptiveMCAPI.Methods;
using PerceptiveMCAPI.Types;

namespace CommandHandlers
{
	internal class MailingListHandler
	{
		private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		private Database Database { get; set; }

		public MailingListHandler() { }

		public bool Process(RequestMailingCommand command, bool sendEmailNotice=true)
		{
			bool mailingListSuccess = false;
			bool mailSuccess =false;

			// TODO: create RequestMailingEvent, then RequestMailingEvent handler
			// TODO: Add Versant Database code here...
			try
			{
				var mailingListEntry = new MailingListEntry(Guid.NewGuid());

				mailingListSuccess = AddUserToMailingList(command);

				var e = new AddedToMailingList
				        	{
								StreamId = mailingListEntry.Id,
								FirstName = command.FirstName,
								LastName = command.LastName,
								Email = command.Email
				        	};

				MueVueEvents.Add(e);
				mailingListEntry.OnReceiving(e);
			}
			catch (Exception ex)
			{
				logger.Error("Failed to add new user to MailChimp", ex);
			}

			if (sendEmailNotice)
			{
				try
				{
					logger.Debug("Calling SmtpMail.send()");
					//SmtpMail.Send("info@muevue.com", "MailingList Request", command.ToString(), command.Email);
					mailSuccess = true;
				}
				catch (Exception ex)
				{
					logger.ErrorException("SmtpEmail.Send() failed: ", ex);
					logger.Error("Failed to send email", ex);
				}
			}

			var success = mailingListSuccess || mailSuccess;

			logger.Info("MailingList - eventstore: {0}, mailing: {1}", mailingListSuccess, mailSuccess);
			logger.Info("Request: {0}", command.ToString());
			return success;
		}

		public bool AddUserToMailingList(RequestMailingCommand command)
		{
			var input = new listSubscribeInput
			            	{
			            		api_Validate = true,
			            		parms =
			            			{
										// TODO: this id needs to come from a config file.
			            				id = "",
			            				// "Interested in MueVue List ID"
			            				email_address = command.Email,
			            				email_type = command.HtmlEmail ? EnumValues.emailType.html : EnumValues.emailType.text,
			            				apikey = MCAPISettings.default_apikey,
			            				double_optin = false,
			            				replace_interests = false,
			            				update_existing = true,
			            				merge_vars = new Dictionary<string, object>
			            				             	{
			            				             		{"OPTIN_IP", command.Ip},
			            				             		{"OPTIN_TIME", DateTime.UtcNow.ToString()},
			            				             		{"FNAME", command.FirstName},
			            				             		{"LNAME", command.LastName}
			            				             	},
			            			},
			            		api_AccessType = EnumValues.AccessType.Serial,
			            		api_OutputType = EnumValues.OutputType.JSON
			            	};
			var cmd = new listSubscribe(input);
			var output = cmd.Execute();
			bool mailingListSuccess = output.result;
			logger.Info(String.Format("MailChimp result - {0}, {1}", mailingListSuccess, output.api_ErrorMessages));
			return mailingListSuccess;
		}
	}
}
