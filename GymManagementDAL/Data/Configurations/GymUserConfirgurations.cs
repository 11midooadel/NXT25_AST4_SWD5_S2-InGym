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
    public class GymUserConfirgurations<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(X => X.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.Property(X => X.Email)
                   .HasColumnType("varchar")
                   .HasMaxLength(100);

            builder.Property(X => X.Phone)
                   .HasColumnType("varchar")
                   .HasMaxLength(11);

            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("GymUserValidEmailCheck", "Email LIKE '_%@_%._%'");
                Tb.HasCheckConstraint("GymUserValidPhoneCheck", "Phone LIKE '01%' and Phone Not Like '%[^0-9]%'");
            });

            // Unique Non Clustered Index
            builder.HasIndex(X => X.Email)
                   .IsUnique();
            builder.HasIndex(X => X.Phone)
                   .IsUnique();

            builder.OwnsOne(X => X.Address, AddressBuilder =>
            {
                AddressBuilder.Property(a => a.BuildingNumber)
                              .HasColumnName("BuildingNumber");

                AddressBuilder.Property(a => a.Street)
                              .HasColumnName("Street")
                              .HasColumnType("varchar")
                              .HasMaxLength(30);
                
                AddressBuilder.Property(a => a.City)
                              .HasColumnType("varchar")
                              .HasMaxLength(30);
            });
        }
    }
}
