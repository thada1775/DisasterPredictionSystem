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
            builder.HasKey(x => x.RegionId);
            builder.HasOne(x => x.Region)
                .WithOne(x => x.AlertSetting)
                .HasForeignKey<AlertSetting>(x => x.RegionId);
        }
    }
}
