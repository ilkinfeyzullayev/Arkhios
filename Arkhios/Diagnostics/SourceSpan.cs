using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Diagnostics
{
    internal readonly record struct SourceSpan(
    SourceLocation Start,
    SourceLocation End);
}
