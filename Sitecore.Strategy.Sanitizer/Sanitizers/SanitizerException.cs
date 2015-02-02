using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Strategy.Sanitizer.Sanitizers
{
    public class SanitizerException : Exception
    {
        public SanitizerException()
            : base()
        {
        }
        public SanitizerException(string message)
            : base(message)
        {
        }
        public SanitizerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
