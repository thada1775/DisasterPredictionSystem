using DisasterPrediction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Infrastructure.Data.Configurations
{
    public class AlertSettingConfiguration : IEntityTypeConfiguration<AlertSetting>
    {
        public void Configure(EntityTypeBuilder<AlertSetting> builder)
        {
            builder.HasKey(x => new { x.RegionId, x.DisasterType });
            builder.HasOne(x => x.Region)
                .WithMany(x => x.AlertSettings)
                .HasForeignKey(x => x.RegionId);
        }
    }
}
