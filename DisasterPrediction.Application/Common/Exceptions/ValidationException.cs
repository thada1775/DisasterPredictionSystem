using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public Dictionary<string, List<string>>? Errors { get; }

        public ValidationException(Dictionary<string, List<string>> errors)
            : base("Validation failed")
        {
            Errors = errors;
        }

        public ValidationException(string message) : base(message) { }
    }
}
