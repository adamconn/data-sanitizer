using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Strategy.Sanitizer.Sanitizers
{
    public class SanitizerRetryException : SanitizerException
    {
        public SanitizerRetryException(int retryLimit)
            : base("A value could not be sanitized after running the generator the number of ties specified by the RetryLimit property")
        {
            this.RetryLimit = retryLimit;
        }
        public int RetryLimit { get; private set; }
    }
}
