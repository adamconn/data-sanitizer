﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Strategy.Sanitizer.Sanitizers
{
    public interface ISanitizer
    {
        bool EnsureUniqueValues { get; }
        int RetryLimit { get; }
        void Reset();
    }
}
