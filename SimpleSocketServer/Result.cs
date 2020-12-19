using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace SimpleSocketServer
{
    public static class Result
    {
        public static Socket Socket { get; set; }
        public static IPEndPoint Point { get; set; }
        public static IDictionary<EndPoint, Socket> Clients { get; set; }

        public static bool StateServer;
        public static IDictionary<EndPoint, IEnumerable<int>> ListNumbers;
    }
}