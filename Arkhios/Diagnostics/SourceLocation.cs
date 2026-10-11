using System;
using System.Collections.Generic;
using System.Text;

namespace Arkhios.Diagnostics
{
    internal readonly record struct SourceLocation(
    int Offset,
    int Line,
    int Column);
}
