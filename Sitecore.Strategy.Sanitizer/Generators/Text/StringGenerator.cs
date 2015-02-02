using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Strategy.Sanitizer.Generators.Text
{
    public class StringGenerator : BaseGenerator<string, string>
    {
        public override string NextValue()
        {
            if (this.Values == null || !this.Values.Any())
            {
                return null;
            }
            var count = this.Values.Count();
            var position = this.Random.Next(0, count);
            return this.Values.Skip(position).Take(1).FirstOrDefault();
        }
    }
}
