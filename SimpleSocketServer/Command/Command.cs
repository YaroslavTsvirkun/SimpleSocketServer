using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using CommandLine;
using TB.ComponentModel;

namespace SimpleSocketServer.Command
{
    public abstract class ACommand
    {
        public ACommand()
        {
            
        }

        public abstract void Execute();

        protected virtual T ParserCommand<T>() where T : class, new()
        {
            var command = CommandLineParser();

            var list = new List<string>();
            list.AddRange(command.Split(' ', StringSplitOptions.RemoveEmptyEntries));

            T option = null; //= new T();

            var parser = new Parser(x => {
                x.AutoHelp = false;
            });

            //parser.Settings.AutoHelp = false;

            var parserArg = parser.ParseArguments(list, typeof(T));


            //if (parserArg.Tag = ParserResultType.Parsed)
            var parserResult = parserArg.WithParsed(opt => 
            { 
                if (opt.IsConvertibleTo<T>())
                {
                    option = opt.As<T>().Value;
                }
                else
                {
                    option = null;
                }
            });
            var parserNotResult =  parserArg.WithNotParsed(opt => option = null);
            
            if (option != null)
            {
                var count = 0;
                var type = option.GetType();
                foreach (var x in type.GetProperties())
                {
                    var val = x.GetValue(option, null);
                    if ((val is null))
                    {
                        count += 1;
                        Console.WriteLine($"No argument specified: {x.Name}");
                    }
                }

                if (count == 0)
                {
                    return option;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private string CommandLineParser()
        {
            string command;
            do
            {
                Console.Write(">");
                command = Console.ReadLine();
            }

            while (command.Length == 0);

            return command;
        }
    }
}