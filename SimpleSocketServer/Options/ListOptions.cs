using CommandLine;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SimpleSocketServer.Options
{
    [Verb("list", true, HelpText = "Server tasks.")]
    public class ListOptions : IOptions, IEqualityComparer<ListOptions>
    {
        [Option("client", Required = true)]
        public string Client { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Client);
        }

        public override bool Equals(object obj)
        {
            return Equals(this, (ListOptions)obj);
        }

        public override int GetHashCode()
        {
            return GetHashCode(this);
        }

        public bool Equals([AllowNull] ListOptions x, [AllowNull] ListOptions y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Client == y.Client;
        }

        public int GetHashCode([DisallowNull] ListOptions obj)
        {
            return (obj.Client != null ? obj.Client.GetHashCode() : 0);
        }
    }
}
