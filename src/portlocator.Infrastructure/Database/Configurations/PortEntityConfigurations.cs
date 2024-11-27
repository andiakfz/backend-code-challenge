using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portlocator.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Infrastructure.Database.Configurations
{
    internal sealed class PortEntityConfigurations : IEntityTypeConfiguration<Port>
    {
        public void Configure(EntityTypeBuilder<Port> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd().HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(u => u.PortName).IsRequired();
            builder.Property(u => u.Latitude)
                   .HasPrecision(9, 6)
                   .IsRequired();
            builder.Property(u => u.Longitude)
                   .HasPrecision(9, 6)
                   .IsRequired();
        }
    }
}
