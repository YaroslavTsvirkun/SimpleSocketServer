using System.Collections.Generic;

namespace SimpleSocketServer.Command
{
    public class ServerCommand
    {
        private List<ACommand> _commandsList;

        public ServerCommand()
        {
            _commandsList = new List<ACommand>
            {
                new RunServerCommand(),
                new ListServerCommand(),
                new StopServerCommand()
            };
        }

        public IReadOnlyCollection<ACommand> Commands
        {
            get => _commandsList.AsReadOnly();
        }
    }
}