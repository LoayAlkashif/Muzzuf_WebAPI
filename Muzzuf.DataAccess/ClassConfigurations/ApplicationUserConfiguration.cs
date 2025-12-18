using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Muzzuf.DataAccess.Entites;

namespace Muzzuf.DataAccess.ClassConfigurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasMany(u => u.JobsAdded)
                .WithOne(j => j.AddedBy)
                .HasForeignKey(j => j.AddedById);

            builder.Property(u => u.ProgrammingLanguages)
                     .HasConversion(
                          v => string.Join(";", v),
                          v => v.Split(";", StringSplitOptions.RemoveEmptyEntries).ToList()
                      );

            builder.Property(u => u.Level).HasConversion<string>().IsRequired(false);

            builder.HasIndex(u => u.NationalId).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}
