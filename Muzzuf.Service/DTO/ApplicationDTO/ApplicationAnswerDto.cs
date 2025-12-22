using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Muzzuf.DataAccess.Enums;

namespace Muzzuf.Service.DTO.ApplicationDTO
{
    public class ApplicationAnswerDto
    {
        public int QuestionId { get; set; }

        public AnswerType AnswerType { get; set; }

        public string? TextAnswer { get; set; }

        public IFormFile? RecordFile { get; set; }
    }
}
