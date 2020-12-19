using SimpleSocketServer.Options;
using System;

namespace SimpleSocketServer.Command
{
    public class StopServerCommand : ACommand
    {
        private StopOptions _option;

        public override void Execute()
        {
            if (Result.StateServer)
            {
                _option = ParserCommand<StopOptions>();

                if (_option != null)
                {
                    if (_option.GetType() == typeof(StopOptions))
                    {
                        if (Result.StateServer)
                        {
                            if (Result.Clients != null)
                            {
                                foreach(var x in Result.Clients)
                                {
                                    if (x.Value.Connected)
                                    {
                                        x.Value.Close();
                                        x.Value.Dispose();
                                    }
                                }


                                Result.Clients.Clear();
                                Result.Clients = null;
                            }

                            if (Result.Socket != null)
                            {
                                Result.Socket.Close();
                                Result.Socket.Dispose();
                                Result.Socket = null;
                            }

                            if (Result.Point != null) Result.Point = null;

                            if (Result.ListNumbers != null)
                            {
                                Result.ListNumbers.Clear();
                                Result.ListNumbers = null;
                            }
                            Result.StateServer = false;

                            Console.WriteLine("The server is stopped!");
                        }
                        else
                        {
                            Console.WriteLine("The server is not running!");
                        }
                    }
                }
            }
        }
    }
}