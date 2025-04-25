using DisasterPrediction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.Common.Interfaces
{
    public interface IApplicationDbContext 
    {
        public DbSet<Region> Regions { get; set; }
        public DbSet<LocationCoordinates> LocationCoordinates { get; set; }
        public DbSet<AlertSetting> AlertSettings { get; set; }
        public DbSet<AlertHistory> AlertHistories { get; set; }
        DbSet<ApplicationUser> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
