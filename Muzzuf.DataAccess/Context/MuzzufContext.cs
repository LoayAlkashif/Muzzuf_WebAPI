using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Muzzuf.DataAccess.ClassConfigurations;
using Muzzuf.DataAccess.Entites;

namespace Muzzuf.DataAccess.Context
{
    public class MuzzufContext : IdentityDbContext<ApplicationUser>
    {
        public MuzzufContext(): base() { }
        public MuzzufContext(DbContextOptions<MuzzufContext> options) : base(options) { }
        
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobQuestion> JobQuestions { get; set; }

        public DbSet<Application> Applications { get; set; }

        public DbSet<ApplicationAnswer> ApplicationAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration<ApplicationUser>(new ApplicationUserConfiguration());
            modelBuilder.ApplyConfiguration<Job> (new JobConfiguration());
            modelBuilder.ApplyConfiguration<Application> (new ApplicationConfiguration());
            modelBuilder.ApplyConfiguration<ApplicationAnswer> (new ApplicationAnswerCofiguration());

            modelBuilder.Entity<JobQuestion>().Property(jq => jq.AnswerType).HasConversion<string>();
        }
    }
}
