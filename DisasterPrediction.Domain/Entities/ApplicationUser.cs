using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string TimeZoneId { get; set; } = "Asia/Bangkok";
        public string? RegionId { get; set; }
        public ICollection<IdentityUserRole<string>> UserRoles { get; set; }
        public ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public ApplicationUser()
        {
            UserRoles = new HashSet<IdentityUserRole<string>>();
            Claims = new HashSet<IdentityUserClaim<string>>();
        }
    }
}
