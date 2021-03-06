using Commands;

namespace CommandHandlers
{
	public static class CommandHandler
	{
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

		public static bool Process(CommandBase command)
		{
			Logger.Info("Received {0} Command: {1}", command.GetType().Name, command);

			if (command is RequestInvitationCommand)
			{
				var database = new Database();
				var invitationHandler = new InvitationHandler();
				return invitationHandler.Process(command as RequestInvitationCommand);
			}
			if (command is RequestMailingCommand)
			{
				var database = new Database();
				var mailingListHandler = new MailingListHandler();
				return mailingListHandler.Process(command as RequestMailingCommand);
			}
			return false;
		}
	}
}