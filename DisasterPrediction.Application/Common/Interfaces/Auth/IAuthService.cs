using DisasterPrediction.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.Common.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<string?> RegisterAsync(RegisterDto model);
        Task<string?> LoginAsync(LoginDto model);
    }
}
