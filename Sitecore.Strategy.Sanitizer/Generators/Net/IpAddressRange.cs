using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Strategy.Sanitizer.Generators.Net
{
    public class IpAddressRange
    {
        public IpAddressRange(IPAddress ip1, IPAddress ip2)
        {
            var c = Compare(ip1, ip2);
            if (c == -1)
            {
                this.Start = ip1;
                this.End = ip2;
            }
            else
            {
                this.Start = ip2;
                this.End = ip1;
            }
        }

        public override bool Equals(object obj)
        {
            var range = obj as IpAddressRange;
            if (range == null)
            {
                return false;
            }
            return (this.GetHashCode() == range.GetHashCode());
        }

        public override int GetHashCode()
        {
            return (this.Start.GetHashCode() ^ this.End.GetHashCode());
        }

        protected int Compare(IPAddress ip1, IPAddress ip2)
        {
            if (ip1.Equals(ip2))
            {
                return 0;
            }
            var bytes1 = ip1.GetAddressBytes();
            var bytes2 = ip2.GetAddressBytes();
            for (var i = 0; i < bytes1.Length; i++)
            {
                if (bytes1[i] == bytes2[i])
                {
                    continue;
                }
                if (bytes1[i] > bytes2[i])
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            return 0;
        }
        public IPAddress Start { get; private set; }
        public IPAddress End { get; private set; }
    }
}
