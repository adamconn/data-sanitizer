using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Strategy.Sanitizer.Generators.Net
{
    public class IpAddressGenerator : IGenerator<IpAddressRange, IPAddress>
    {
        public IpAddressGenerator()
        {
            this.Random = new Random(Guid.NewGuid().GetHashCode());
            var values = new List<IpAddressRange>();
            values.AddRange(new[]
            {
                new IpAddressRange(IPAddress.Parse("0.0.0.0"), IPAddress.Parse("10.0.0.0")),
                new IpAddressRange(IPAddress.Parse("10.255.255.255"), IPAddress.Parse("172.16.0.0")),
                new IpAddressRange(IPAddress.Parse("172.31.255.255"), IPAddress.Parse("192.168.0.0")),
                new IpAddressRange(IPAddress.Parse("192.168.255.255"), IPAddress.Parse("255.255.255.255"))
            });
            this.DefaultIpAddressRanges = values;
        }
        public IEnumerable<IpAddressRange> DefaultIpAddressRanges { get; set; }
        protected Random Random { get; private set; }
        protected IEnumerable<IpAddressRange> Values { get; set; }

        public virtual void AddValues(params IpAddressRange[] ranges)
        {
            if (ranges == null || ranges.Length == 0)
            {
                return;
            }
            if (this.Values == null)
            {
                this.Values = Values;
            }
            else
            {
                this.Values = this.Values.Concat(ranges);
            }
        }

        public virtual void RemoveValues(params IpAddressRange[] values)
        {
            if (values == null || values.Length == 0 || this.Values == null || !this.Values.Any())
            {
                return;
            }
            this.Values = this.Values.Except(values).ToList();
        }

        public virtual IPAddress NextValue()
        {
            var range = GetRandomRange();
            var address = GetRandomIpAddress(range);
            return address;
        }

        protected virtual IpAddressRange GetRandomRange()
        {
            var values = this.Values;
            if (values == null || !values.Any())
            {
                values = this.DefaultIpAddressRanges;
            }
            var count = values.Count();
            var position = this.Random.Next(0, count);
            var range = values.Skip(position).Take(1).FirstOrDefault();
            return range;
        }
        protected virtual IPAddress GetRandomIpAddress(IpAddressRange range)
        {
            var start = range.Start.GetAddressBytes();
            var end = range.End.GetAddressBytes();
            var bytes = new byte[4];
            for (var i = 0; i < 4; i++)
            {
                bytes[i] = GetRandomByteInRange(start[i], end[i]);
            }
            return new IPAddress(bytes);
        }
        protected virtual byte GetRandomByteInRange(byte b1, byte b2)
        {
            if (b1 == b2)
            {
                return b1;
            }
            var b3 = (b1 > b2) ? b2 : b1;
            var diff = Math.Abs(b1 - b2);
            var r = this.Random.Next(0, (diff + 1));
            return (byte)(b3 + r);
        }
    }
}
