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
    public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.HasOne(a => a.Employee)
                .WithMany(u => u.Applications)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Job)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(a => a.Status).HasConversion<string>();

            builder.Navigation(a => a.Answers).AutoInclude(false);
        }
    }
}
