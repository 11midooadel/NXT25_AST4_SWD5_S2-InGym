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
    public class MemberConfigurations : GymUserConfirgurations<Member>,IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            base.Configure(builder);
            builder.Property(X => X.CreateAt)
                   .HasColumnName("JoinDate")
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}
