using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inbox2.Platform.Interfaces
{
    public abstract class ITracer
    {
        public abstract Guid GetTacerId();

        public abstract long GetUserId();
    }
}
