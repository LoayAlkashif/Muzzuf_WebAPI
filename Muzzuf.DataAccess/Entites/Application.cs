using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muzzuf.DataAccess.Enums;

namespace Muzzuf.DataAccess.Entites
{
    public class Application
    {
        public int Id { get; set; }

        public string EmployeeId { get; set; }
        public ApplicationUser Employee {  get; set; }
        public int JobId { get; set; }
        public Job? Job { get; set; }

        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

        public ICollection<ApplicationAnswer> Answers { get; set; } = new List<ApplicationAnswer>();

        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

    }
}
