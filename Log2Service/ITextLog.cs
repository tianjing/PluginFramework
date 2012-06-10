using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Log2Service
{
    public interface ITextLog
    {
        void OutLog(string message);
        void OutLog(string message, Exception e);

    }
}
