using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muzzuf.DataAccess.Enums;

namespace Muzzuf.Service.DTO.JobDTO
{
    public class JobQuestionDto
    {
        public string QuestionName { get; set; }

        public AnswerType AnswerType { get; set; }
    }
}
