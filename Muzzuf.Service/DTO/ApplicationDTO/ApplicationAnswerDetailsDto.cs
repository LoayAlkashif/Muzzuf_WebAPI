using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Muzzuf.DataAccess.Enums;

namespace Muzzuf.Service.DTO.ApplicationDTO
{
    public class ApplicationAnswerDetailsDto
    {
        public string QuestionName { get; set; }
        public AnswerType AnswerType { get; set; }

        public string? TextAnswer { get; set; }
        public string? RecordUrl { get; set; }
    }

}
