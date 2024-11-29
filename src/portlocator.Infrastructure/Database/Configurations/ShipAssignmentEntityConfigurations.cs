using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using portlocator.Domain.ShipAssignments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Infrastructure.Database.Configurations
{
    internal sealed class ShipAssignmentEntityConfigurations : IEntityTypeConfiguration<ShipAssignment>
    {
        public void Configure(EntityTypeBuilder<ShipAssignment> builder)
        {
            builder.HasKey(u => new { u.UserId, u.ShipId });

            builder.HasOne(u => u.User)
                   .WithMany(u => u.ShipAssignments)
                   .HasForeignKey(u => u.UserId);
            builder.HasOne(u => u.Ship)
                   .WithMany(u => u.ShipAssignments)
                   .HasForeignKey(u => u.ShipId);
        }
    }
}
