using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.Common.BaseClass
{
    public class BaseFilterDto
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string Keyword { get; set; } = string.Empty;
    }
}
