using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muzzuf.DataAccess.Entites
{
    public class ApplicationAnswer
    {
        public int Id { get; set; }

        public int ApplicationId { get; set; }
        public Application Application { get; set; }

        public int QuestionId { get; set; }
        public JobQuestion Question { get; set; }

        public string? TextAnswer { get; set; }

        public string? RecordAnswerUrl { get; set; }

    }
}
