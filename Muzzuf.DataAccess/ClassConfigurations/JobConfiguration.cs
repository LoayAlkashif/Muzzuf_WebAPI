using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Muzzuf.DataAccess.Entites;

namespace Muzzuf.DataAccess.ClassConfigurations
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {

            builder.HasOne(j => j.AddedBy)
                .WithMany(u => u.JobsAdded)
                .HasForeignKey(j => j.AddedById)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(j => j.Questions)
                .WithOne(q => q.Job)
                .HasForeignKey(q => q.JobId)
                .OnDelete(DeleteBehavior.Cascade);

          
            builder.Property(j => j.Level).HasConversion<string>();

            builder.Property(j => j.RequiredLanguage)
                   .HasConversion(
                        v => string.Join(";", v),
                        v => v.Split(";", StringSplitOptions.RemoveEmptyEntries).ToList()
                   );

        }
    }
}
