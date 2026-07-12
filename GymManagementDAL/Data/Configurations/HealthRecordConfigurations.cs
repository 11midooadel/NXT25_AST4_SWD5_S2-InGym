using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    public class HealthRecordConfigurations : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.ToTable("Members")
                   .HasKey(X => X.Id); // Not Needed [By Convention]

            builder.HasOne<Member>()
                    .WithOne(X => X.HealthRecord)
                     .HasForeignKey<HealthRecord>(X => X.Id);

            builder.Ignore(X => X.CreateAt);
            builder.Ignore(X => X.UpdatedAt);
        }
    }
}
