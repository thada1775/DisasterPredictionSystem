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
    public class LocationCoordinatesConfiguration : IEntityTypeConfiguration<LocationCoordinates>
    {
        public void Configure(EntityTypeBuilder<LocationCoordinates> builder)
        {
            builder.HasKey(x => x.RegionId);
            builder.HasOne(x => x.Region)
                .WithOne(x => x.LocationCoordinates)
                .HasForeignKey<LocationCoordinates>(x => x.RegionId);
        }
    }
}
