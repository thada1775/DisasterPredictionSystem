using DisasterPrediction.Application.Common.Interfaces;
using DisasterPrediction.Application.Common.Interfaces.Auth;
using DisasterPrediction.Application.DTOs.Auth;
using DisasterPrediction.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Infrastructure.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;

        public AuthService(UserManager<ApplicationUser> userManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<string?> RegisterAsync(RegisterDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return null;

            var userRoles = await _userManager.GetRolesAsync(user);

            return _jwtService.GenerateToken(user.Id, user.Email, userRoles);
        }

        public async Task<string?> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password) || string.IsNullOrWhiteSpace(user.Email))
                return null;

            var userRoles = await _userManager.GetRolesAsync(user);

            return _jwtService.GenerateToken(user.Id, user.Email, userRoles);
        }
    }
}
