using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Errors
{
    internal abstract class ArkhiosException : Exception
    {
        protected ArkhiosException(string message)
            : base(message)
        {
        }
    }
}
