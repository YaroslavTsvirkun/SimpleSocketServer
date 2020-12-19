using CommandLine;

namespace SimpleSocketServer.Options
{
    [Verb("stop", true, HelpText = "Stops a running server.")]
    public class StopOptions : IOptions
    {
        public bool IsValid()
        {
            return true;
        }
    }
}