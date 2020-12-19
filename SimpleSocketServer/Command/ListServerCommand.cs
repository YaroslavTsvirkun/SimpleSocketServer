using SimpleSocketServer.Options;
using System;
using System.Linq;

namespace SimpleSocketServer.Command
{
    public class ListServerCommand : ACommand
    {
        private ListOptions _option;

        public override void Execute()
        {
            if (Result.StateServer)
            {
                _option = ParserCommand<ListOptions>();

                if (_option != null)
                {
                    if (_option.GetType() == typeof(ListOptions))
                    {
                        if (Result.ListNumbers != null)
                        {
                            if (_option.Client == "all")
                            {
                                foreach (var s in Result.ListNumbers)
                                {
                                    Console.WriteLine($"Client: {s.Key}, Sum of digits entered: {s.Value.Sum()}");
                                }
                            }
                            else
                            {
                                try
                                {
                                    var count = Convert.ToInt32(_option.Client);
                                    if (count <= Result.ListNumbers.Count - 1)
                                    {
                                        var list = Result.ListNumbers.Take(count);

                                        foreach (var s in list)
                                        {
                                            Console.WriteLine($"Client: {s.Key}, Sum of digits entered: {s.Value.Sum()}");
                                        }
                                    }
                                }
                                catch (FormatException ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("List empty!");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("The server is not running!");
            }
        }
    }
}