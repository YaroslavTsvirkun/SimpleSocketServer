using SimpleSocketServer.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SimpleSocketServer.Command
{
    public class RunServerCommand : ACommand
    {
        private RunOptions _option;

        public RunServerCommand()
        {
            
        }

        public override void Execute()
        {
            if (Result.StateServer == false)
            {
                _option = ParserCommand<RunOptions>();

                if (_option != null)
                {
                    if (!Result.StateServer)
                    {
                        //if (Result.Socket != null) Result.Socket.Dispose();

                        if (Result.Point == null && Result.Socket == null)
                        {
                            Result.StateServer = true;
                            Result.Point = new IPEndPoint(_option.GetHost(), _option.Port);
                            Result.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        }

                        try
                        {
                            Result.Socket.Bind(Result.Point);
                            Result.Socket.Listen(10);

                            Console.WriteLine("The server is running. Waiting for connections...");

                            void Task()
                            {
                                var input = true;
                                do
                                {
                                    if (Result.Socket != null)
                                    {
                                        Socket handler = null;
                                        try
                                        {
                                            handler = Result.Socket.Accept();
                                            if (Result.Clients == null)
                                            {
                                                Result.Clients = new Dictionary<EndPoint, Socket>();
                                            }

                                            if (handler.RemoteEndPoint != null) Result.Clients.Add(handler.RemoteEndPoint, handler);

                                            StringBuilder builder = new StringBuilder();
                                            int bytes = 0;
                                            byte[] data = new byte[256];
                                            var exeption = 0;

                                            do
                                            {
                                                Result.Clients.TryGetValue(handler.RemoteEndPoint, out var h);
                                                bytes = h.Receive(data);
                                                var str = Encoding.UTF8.GetString(data, 0, bytes);
                                                builder.Append(str);

                                                if (str == "q")
                                                {
                                                    //input = false;
                                                    h.Shutdown(SocketShutdown.Both);
                                                    //h.Close();
                                                    break;
                                                }
                                                else
                                                {
                                                    var address = h.RemoteEndPoint;
                                                    IEnumerable<int> ConvertString(string val, Socket x)
                                                    {
                                                        var chr = new char[] { ' ', '\r', '\n' };
                                                        var result = str.Split(chr, StringSplitOptions.RemoveEmptyEntries);

                                                        var list = new List<int>();
                                                        foreach (var t in result)
                                                        {
                                                            try
                                                            {
                                                                list.Add(Convert.ToInt32(t));
                                                            }
                                                            catch (FormatException ex)
                                                            {
                                                                exeption++;
                                                                var data = Encoding.Unicode.GetBytes(ex.Message);
                                                                x.Send(data);
                                                            }
                                                        }

                                                        return list;
                                                    }

                                                    if (Result.ListNumbers == null) Result.ListNumbers = new Dictionary<EndPoint, IEnumerable<int>>();

                                                    if (Result.ListNumbers.TryGetValue(address, out var vs))
                                                    {
                                                        var values = vs.ToList();
                                                        values.AddRange(ConvertString(str, h));

                                                        Result.ListNumbers.Remove(address);
                                                        Result.ListNumbers.Add(address, values);
                                                    }
                                                    else
                                                    {
                                                        Result.ListNumbers.Add(address, ConvertString(str, handler));
                                                    }
                                                }
                                            }
                                            while (input);

                                            if (exeption > 0)
                                            {
                                                Result.Clients.TryGetValue(handler.RemoteEndPoint, out var h);
                                                if (h.Connected)
                                                {
                                                    string message = "This command is not supported by the server!";
                                                    data = Encoding.Unicode.GetBytes(message);
                                                    h.Send(data);
                                                }

                                            }
                                            else
                                            {
                                                if(handler != null)
                                                {
                                                    Result.Clients.TryGetValue(handler.RemoteEndPoint, out var h);
                                                    if (h.Connected)
                                                    {
                                                        string message = "Your command has been processed by the server!";
                                                        data = Encoding.Unicode.GetBytes(message);
                                                        h.Send(data);
                                                    }
                                                }


                                            }
                                        }
                                        catch (SocketException ex) when (ex.ErrorCode == 10004)
                                        {
                                            if(handler != null)
                                            {
                                                Result.Clients.TryGetValue(handler.RemoteEndPoint, out var h);
                                                string message = "Your command has been processed by the server!";
                                                var data = Encoding.Unicode.GetBytes(message);
                                                h.Send(data);
                                            }
                                            return;
                                        }
                                    }

                                } while (input);
                            }

                            var thread = new Thread(Task);
                            thread.Start();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("The server is already running!");
                    }
                }
            }
        }
    }
}