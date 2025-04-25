using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.Common.Utils
{
    public static class ListUtil
    {
        public static bool IsEmptyList<T>([NotNullWhen(false)] ICollection<T>? lst) => lst == null || !lst.Any();
    }
}
