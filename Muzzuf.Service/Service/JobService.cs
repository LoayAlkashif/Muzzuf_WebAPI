using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Muzzuf.DataAccess.Entites;
using Muzzuf.DataAccess.IRepository;
using Muzzuf.Service.CustomError;
using Muzzuf.Service.DTO.JobDTO;
using Muzzuf.Service.Helpers;
using Muzzuf.Service.IService;

namespace Muzzuf.Service.Service
{
    public class JobService : IJobService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJobRepository _jobRepo;
        private readonly IPaginationService _paginationService;

        public JobService(UserManager<ApplicationUser> userManager, IJobRepository jobRepo, IPaginationService paginationService)
        {
            _userManager = userManager;
            _jobRepo = jobRepo;
            _paginationService = paginationService;
        }

        private JobDto jobResponse(Job job,string currentUserId = null, string employerName = null)
        {
            var app = job.Applications.FirstOrDefault(a => a.EmployeeId == currentUserId);
            return new JobDto
            {
                Id = job.Id.ToString(),
                EmployerName = employerName ?? job.AddedBy?.FullName,
                CompanyName = job.AddedBy?.CompanyName,
                CompanyLogo = job.AddedBy?.CompanyLogoUrl,
                Title = job.Title,
                Description = job.Description,
                Region = job.Region,
                City = job.City,
                RequiredLanguage = job.RequiredLanguage,
                Level = job.Level,
                IsActive = job.IsActive,
                ApplicationStatus = app?.Status,
                Questions = job.Questions.Select(q => new JobQuestionDto
                {
                    Id = q.Id,
                    QuestionName = q.QuestionName,
                    AnswerType = q.AnswerType
                }).ToList()
            };

        }

        public async Task<JobDto> CreateJobAsync(string employerId, CreateUpdateJobDto dto)
        {
            var employer = await _userManager.FindByIdAsync(employerId);
            if (employer == null)
                throw new NotFoundException("User not Found");

            var job = new Job
            {
                Title = dto.Title,
                Description = dto.Description,
                Region = dto.Region,
                City = dto.City,
                RequiredLanguage = dto.RequiredLanguage,
                AddedById = employer.Id,
                Level = dto.Level,
                Questions = dto.Questions?.Select(q => new JobQuestion
                {

                      QuestionName = q.QuestionName,
                      AnswerType = q.AnswerType
                }).ToList()
            };


            await _jobRepo.AddAsync(job);
            await _jobRepo.SaveAsync();

            job = await _jobRepo.GetByIdWithQuestionsAsync(job.Id);

            return jobResponse(job);
        }

        public async Task DeleteJobAsync(int jobId, string employerId)
        {
            var job = await _jobRepo.GetByIdAsync(jobId);
            if (job == null)
                throw new NotFoundException("Job Not Found");

            if (job.AddedById != employerId)
                throw new NotAuthorizeException("You are not able to Delete this Job");

            _jobRepo.Delete(job);
            await _jobRepo.SaveAsync();
        }

        public async Task<PagedResult<JobDto>> GetActiveJobsAsync(string query, int page, int limit)
        {
            var jobsQuery = _jobRepo.GetActiveJobsAsync();

            if(!string.IsNullOrWhiteSpace(query))
            {
                jobsQuery = jobsQuery.Where(j => j.Title.Contains(query) || j.City.Contains(query));
            }

            var dtoQuery = jobsQuery.OrderByDescending(j => j.Id)
                .Select(j => new JobDto
                {
                    Id = j.Id.ToString(),
                    EmployerName = j.AddedBy.FullName,
                    CompanyName = j.AddedBy.CompanyName,
                    CompanyLogo = j.AddedBy.CompanyLogoUrl,
                    Title = j.Title,
                    Region = j.Region,
                    City = j.City,
                    Description = j.Description,
                    RequiredLanguage = j.RequiredLanguage,
                    Level = j.Level,
                    IsActive = j.IsActive,
                });

            return await _paginationService.PaginateAsync(dtoQuery, page, limit);
        }

        public async Task<PagedResult<JobDto>> GetEmployerJobsAsync(string employerId, string query, int page, int limit)
        {
            var jobsQuery = _jobRepo.GetJobsByEmployerAsync(employerId);
            if (!string.IsNullOrWhiteSpace(query))
            {
                jobsQuery = jobsQuery.Where(j => j.Title.Contains(query) || j.City.Contains(query));
            }

            var dtoQuery = jobsQuery.OrderByDescending(j => j.Id)
                .Select(j => new JobDto
                {
                    Id = j.Id.ToString(),
                    EmployerName = j.AddedBy.FullName,
                    CompanyName = j.AddedBy.CompanyName,
                    CompanyLogo = j.AddedBy.CompanyLogoUrl,
                    Title = j.Title,
                    Region = j.Region,
                    City = j.City,
                    Description = j.Description,
                    RequiredLanguage = j.RequiredLanguage,
                    Level = j.Level,
                    IsActive = j.IsActive
                });

            return await _paginationService.PaginateAsync(dtoQuery, page, limit);
        }

        public async Task<JobDto> GetJobByIdAsyc(int jobId, string currentUserId)
        {
            var job = await _jobRepo.GetByIdWithQuestionsAsync(jobId);
            if (job == null)
                throw new NotFoundException("Job Not Found");
            return jobResponse(job, currentUserId);
        }

        public async Task<JobDto> UpdateJobAsync(int jobId, string employerId, CreateUpdateJobDto dto)
        {
            var job = await _jobRepo.GetByIdAsync(jobId);
            if (job == null)
                throw new NotFoundException("Job Not Found");

            if (job.AddedById != employerId)
                throw new NotAuthorizeException("You can't edit this job");

            job.Title = dto.Title;
            job.Description = dto.Description;
            job.Region = dto.Region;
            job.City = dto.City;
            job.Level = dto.Level;
            job.RequiredLanguage = dto.RequiredLanguage;

            if(dto.Questions != null)
            {
                var QuestionsIds = dto.Questions.Where(q => q.Id.HasValue)
                .Select(q => q.Id.Value).ToList();

                var RemovedQuestions = job.Questions.Where(q => !QuestionsIds.Contains(q.Id)).ToList();

                foreach (var q in RemovedQuestions)
                {
                    job.Questions.Remove(q);
                }

                foreach (var questionDto in dto.Questions)
                {
                    if(questionDto.Id == null)
                    {
                        job.Questions.Add(new JobQuestion
                        {
                            QuestionName = questionDto.QuestionName,
                            AnswerType = questionDto.AnswerType,
                        });
                    }
                    else
                    {
                        var existingQuestion = job.Questions.FirstOrDefault(q => q.Id == questionDto.Id);

                        if(existingQuestion != null)
                        {
                            existingQuestion.QuestionName = questionDto.QuestionName;
                            existingQuestion.AnswerType = questionDto.AnswerType;
                        }
                    }
                }
            }

            _jobRepo.Update(job);
            await _jobRepo.SaveAsync();
            return jobResponse(job);
        }

        public async Task DeActiveJobAsync(int jobId, string employerId)
        {
            var job = await _jobRepo.GetByIdAsync(jobId);
            if (job == null)
                throw new NotFoundException("Job Not Found");

            if (job.AddedById != employerId)
                throw new NotAuthorizeException("You can't DeActive this job");

            if (!job.IsActive)
                throw new BadRequestException("This job is already deactivated");

            job.IsActive = false;
            _jobRepo.Update(job);
            await _jobRepo.SaveAsync();
            
        }



    }
}
