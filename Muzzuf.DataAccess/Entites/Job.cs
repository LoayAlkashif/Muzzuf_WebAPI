using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muzzuf.DataAccess.Enums;

namespace Muzzuf.DataAccess.Entites
{
    public class Job
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public JobLevel Level { get; set; }

        public List<string> RequiredLanguage { get; set; } = new();

        public bool IsActive { get; set; } = true;

        public string AddedById { get; set; }
        public ApplicationUser? AddedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<JobQuestion>? Questions { get; set; }
        public ICollection<Application>? Applications { get; set; }
    }
}
