using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.Common.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string userId, string email, IList<string> userRoles);
    }
}
