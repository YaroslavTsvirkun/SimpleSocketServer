using CommandLine;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using TB.ComponentModel;

namespace SimpleSocketServer.Options
{
    [Verb("run", true, HelpText = "Starts the server at the specified address and port.")]
    public class RunOptions : IOptions, IEqualityComparer<RunOptions>
    {
        [Option("host", Required = true)]
        public string Host { get; set; }

        [Option("port", Required = true)]
        public ushort Port { get; set; }

        public IPAddress GetHost()
        {
            return IPAddress.Parse(this.Host);
        }

        public bool IsValid()
        {
            return GetHost() != null;
        }

        public override bool Equals(object obj)
        {
            return Equals(this, obj.As<RunOptions>());
        }

        public override int GetHashCode()
        {
            return GetHashCode(this);
        }

        public bool Equals([AllowNull] RunOptions x, [AllowNull] RunOptions y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.GetHost() == y.GetHost() && x.Port == y.Port;
        }

        public int GetHashCode([DisallowNull] RunOptions obj)
        {
            return (obj.GetHost() != null ? obj.GetHost().GetHashCode() : 0) + (obj.Port < 0 ? obj.Port.GetHashCode() : 0);
        }
    }
}