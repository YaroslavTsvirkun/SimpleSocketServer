using SimpleSocketServer.Command;
using System;

namespace SimpleSocketServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the command to start the server:");

            var commands = new ServerCommand();

            while (true)
            {
                foreach(var command in commands.Commands)
                {
                    command.Execute();
                }
            }
        }
    }
}