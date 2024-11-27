using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portlocator.Domain.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Infrastructure.Database.Configurations
{
    internal sealed class ShipEntityConfigurations : IEntityTypeConfiguration<Ship>
    {
        public void Configure(EntityTypeBuilder<Ship> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd().HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(u => u.ShipName).IsRequired();

            builder.Property(u => u.Velocity)
                   .HasPrecision(10, 2)
                   .IsRequired();

            builder.Property(u => u.Latitude)
                   .HasPrecision(9, 6)
                   .IsRequired();

            builder.Property(u => u.Longitude)
                   .HasPrecision(9,6)
                   .IsRequired();
        }
    }
}
