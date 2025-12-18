using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muzzuf.DataAccess.Enums;

namespace Muzzuf.DataAccess.Entites
{
    public class JobQuestion
    {
        public int Id { get; set; }

        public string QuestionName { get; set; }

        public AnswerType AnswerType { get; set; }

        public int? JobId { get; set; }
        public Job? Job { get; set; }

    }
}
