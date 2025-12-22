using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Muzzuf.DataAccess.Entites;
using Muzzuf.DataAccess.Enums;
using Muzzuf.DataAccess.IRepository;
using Muzzuf.Service.CustomError;
using Muzzuf.Service.DTO.ApplicationDTO;
using Muzzuf.Service.Helpers;
using Muzzuf.Service.IService;

namespace Muzzuf.Service.Service
{
    public class ApplicationService : IApplicationService
    {
        private readonly IJobRepository _jobRepo;
        private readonly IFileService _fileService;
        private readonly IApplicationRepository _applicationRepo;
        private readonly IEmailService _emailService;
        private readonly IPaginationService _paginationService;

        public ApplicationService(IJobRepository jobRepo,
            IFileService fileService, IApplicationRepository applicationRepo,
            IEmailService emailService, IPaginationService paginationService)
        {
            _jobRepo = jobRepo;
            _fileService = fileService;
            _applicationRepo = applicationRepo;
            _emailService = emailService;
            _paginationService = paginationService;
        }



        private static readonly string[] AllowedAudioTypes =
                                        {
                                            "audio/mpeg",
                                            "audio/wav",
                                            "audio/webm",
                                            "audio/ogg",
                                            "audio/m4a",
                                            "audio/x-m4a",
                                            "audio/mp4"
                                        };




        public async Task<PagedResult<ApplicationListDto>> GetJobApplicationsAsync(int jobId, string employerId, string query, int page, int limit)
        {

            var job = await _jobRepo.GetByIdAsync(jobId)
                ?? throw new BadRequestException("Job Not Found");


            if (job.AddedById != employerId)
                throw new BadRequestException("Access denied");

            var applicationsQuery = _applicationRepo.GetJobApplicationsQueryable(jobId)
                .Where(a => string.IsNullOrEmpty(query) || a.Employee.FullName.Contains(query))
                .OrderByDescending(a => a.AppliedAt)
                .Select(a => new ApplicationListDto
                {
                    Id = a.Id,
                    EmployeeId = a.EmployeeId,
                    EmployeeName = a.Employee.FullName,
                    ProfileImageUrl = a.Employee.ProfileImageUrl ?? null,
                    Status = a.Status,
                    AppliedAt = a.AppliedAt
                });


                return await _paginationService.PaginateAsync(applicationsQuery, page, limit);



        }


        public async Task ApplyJobAsync(string employeeId, ApplyJobDto dto)
        {
            var job = await _jobRepo.GetByIdWithQuestionsAsync(dto.JobId) ?? throw new BadRequestException("Job is not Found");



            if (!job.IsActive)
                throw new BadRequestException("Job already closed");

            var existingApplication = await _applicationRepo.HasUserAppliedAsync(employeeId, dto.JobId);

            if (existingApplication)
                throw new BadRequestException("You have already applied for this job");


            var app = new Application
            {
                JobId = dto.JobId,
                EmployeeId = employeeId,
                Answers = new List<ApplicationAnswer>()
            };

            foreach(var q in job.Questions)
            {
                var answerDto = dto.Answers.FirstOrDefault(a => a.QuestionId == q.Id);
                if(q.AnswerType == AnswerType.Record)
                {

                    if (answerDto?.RecordFile == null)
                        throw new BadRequestException($"Record is required for question: {q.QuestionName}");

                    if (!AllowedAudioTypes.Contains(answerDto.RecordFile.ContentType))
                        throw new BadRequestException("Invalid audio format");

                    var allowedExtensions = new[] { ".mp3", ".wav", ".webm", ".ogg", ".m4a" };
                    var ext = Path.GetExtension(answerDto.RecordFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(ext))
                        throw new BadRequestException("Invalid audio file extension");

                    if (answerDto.RecordFile.Length > 5 * 1024 * 1024)
                        throw new BadRequestException("Audio file is too large (max 5MB)");

                    var fileUrl = await _fileService.UploadAsync(answerDto.RecordFile, "Records");

                    app.Answers.Add(new ApplicationAnswer
                    {
                        QuestionId = q.Id,
                        RecordAnswerUrl = fileUrl
                    });
                }
                else
                {
                    app.Answers.Add(new ApplicationAnswer
                    {
                        QuestionId = q.Id,
                        TextAnswer = answerDto.TextAnswer
                    });
                }
            }

            await _applicationRepo.AddAsync(app);
            await _applicationRepo.SaveAsync();
        }

       
        public async Task AcceptApplicationAsync(int applicationId, string employerId)
        {

            var app = await _applicationRepo.GetApplicationWithAnswersAsync(applicationId)
                ?? throw new NotFoundException("Application Not Found");

            if (app.Job.AddedById != employerId)
                throw new NotAuthorizeException("Access Denied");

            app.Status = ApplicationStatus.Accepted;

            await _applicationRepo.SaveAsync();

            await _emailService.SendAsync(app.Employee.Email,
                $"Your Update from {app.Job.AddedBy.CompanyName}",
                $"<h3>Congrats!</h3><p>You have been accepted for {app.Job.Title}</p>"
                );


        }

        public async Task RejectApplicationAsync(int applicationId, string employerId)
        {

            var app = await _applicationRepo.GetApplicationWithAnswersAsync(applicationId)
                ?? throw new NotFoundException("Application Not Found");


            if (app.Job.AddedById != employerId)
                throw new NotAuthorizeException("Access Denied");


            foreach(var answer in app.Answers)
            {
                if(!string.IsNullOrEmpty(answer.RecordAnswerUrl))
                {
                    _fileService.Delete(answer.RecordAnswerUrl);
                    answer.RecordAnswerUrl = null;
                }
            }

            app.Status = ApplicationStatus.Rejected;
            await _applicationRepo.SaveAsync();

            await _emailService.SendAsync(
                app.Employee.Email,
                $"Your Update from {app.Job.AddedBy.CompanyName}",
                $"<p>Thank you for your interest in the {app.Job.Title} position at {app.Job.AddedBy.CompanyName} in {app.Job.AddedBy.City}, {app.Job.AddedBy.Region}." +
                $" Unfortunately, we will not be moving forward with your application, but we appreciate your time and interest in {app.Job.AddedBy.CompanyName}.</p>"
            );
        }

        public async Task<ApplicationDetailsDto> GetApplicationDetailsAsync(int appId, string employerId)
        {
            var app = await _applicationRepo.GetApplicationWithAnswersAsync(appId)
                ?? throw new NotFoundException("Application Not Found");

            if (app.Job.AddedById != employerId)
                throw new NotAuthorizeException("Access Denied");

            return new ApplicationDetailsDto
            {
                ApplicationId = app.Id,
                EmployeeName = app.Employee.FullName,
                JobTitle = app.Job.Title,
                Status = app.Status,
                Answers = app.Answers.Select(a => new ApplicationAnswerDetailsDto
                {
                    QuestionName = a.Question.QuestionName,
                    AnswerType = a.Question.AnswerType,
                    TextAnswer = a.TextAnswer,
                    RecordUrl = a.RecordAnswerUrl
                }).ToList()
            };
        }
    }
}
