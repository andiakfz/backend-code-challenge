using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portlocator.Domain.ShipCrews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Infrastructure.Database.Configurations
{
    internal sealed class ShipCrewEntityConfigurations : IEntityTypeConfiguration<ShipCrew>
    {
        public void Configure(EntityTypeBuilder<ShipCrew> builder)
        {
            builder.HasKey(u => new { u.UserId, u.ShipId });

            builder.HasOne(u => u.User)
                   .WithMany(u => u.ShipCrews)
                   .HasForeignKey(u => u.UserId);
            builder.HasOne(u => u.Ship)
                   .WithMany(u => u.ShipCrews)
                   .HasForeignKey(u => u.ShipId);
        }
    }
}
