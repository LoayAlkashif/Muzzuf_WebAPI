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
    public class ApplicationAnswerCofiguration : IEntityTypeConfiguration<ApplicationAnswer>
    {
        public void Configure(EntityTypeBuilder<ApplicationAnswer> builder)
        {
            builder.HasOne(a => a.Application)
                .WithMany(a => a.Answers)
                .HasForeignKey(a => a.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Question)
                .WithMany()
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
