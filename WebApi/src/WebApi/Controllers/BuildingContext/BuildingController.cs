using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BuildingContext.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Database.Repositories.Interfaces;
using WebApi.Domain.Models;
using WebApi.Infrastructure;
using WebApi.Services;

namespace WebApi.Controllers.BuildingContext
{
    [Authorize]
    [Route("api/[controller]")]
    public class BuildingController: BuildingContextController
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IMailService _mailService;

        public BuildingController(IBuildingRepository buildingRepository, ICompanyRepository companyRepository, IUserRepository userRepository, ICurrentUser currentUser, IMailService mailService) : base(buildingRepository)
        {
            _buildingRepository = buildingRepository;
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _currentUser = currentUser;
            _mailService = mailService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var buildingId = new BuildingId(id);
            var building = _buildingRepository.Get(buildingId);
            var canReport = _currentUser.User.CanReport(buildingId);

            return Ok(new 
            {
                Name = building.Name,
                CanReport = canReport
            });
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]DTO.BuildingCreation buildingCreation)
        {
            var company = new Company(
                new CompanyId(buildingCreation.Company.Id),
                buildingCreation.Company.Name,
                buildingCreation.Company.Nip,
                buildingCreation.Company.PhoneNumber,
                buildingCreation.Company.Email,
                buildingCreation.Company.City,
                buildingCreation.Company.PostCode,
                buildingCreation.Company.Street,
                buildingCreation.Company.PlaceNumber
            );

            var building = new Building(new BuildingId(id), buildingCreation.Name, company, new List<Company>());
            _buildingRepository.Save(building);
            _companyRepository.Save(company);
            return Ok();
        }

        [HttpPost("{id}/mainReports/")]
        public IActionResult GetMainReport(int id, [FromBody] DateRange dateRange)
        {
            var buildingId = new BuildingId(id);
            var estimationRepository = GetBulidingRepositoryFactory(buildingId).Estimation;
            var companies = _companyRepository.GetContractors(buildingId);

            IEnumerable<EstimationReportList> reports;
            if (dateRange.From.Year == 1 || dateRange.To.Year == 1)
            {
                reports = estimationRepository.GetAll();
            }
            else
            {
                reports = estimationRepository.GetFrom(dateRange.From, dateRange.To);
            }

            var result = reports.Select(r => new
            {
                EstimationId = r.Estimation.EstimationId,
                SpecNumber = r.Estimation.SpecNumber,
                Description = r.Estimation.Description,
                Unit = r.Estimation.Unit,
                Quantity = r.Estimation.Quantity,
                UnitPrice = r.Estimation.UnitPrice,
                Amount = r.Estimation.Amount,
                DoneQuantity = r.Reports.Sum(rq => rq.Quantity)
            });

            return Ok(result);
        }

        [HttpGet("{id}/report/{reportId}")]
        public IActionResult GetReport(int id, int reportId)
        {
            var buildingId = new BuildingId(id);
            var reportRepository = GetBulidingRepositoryFactory(buildingId).Report;

            var report = reportRepository.Get(reportId);

            if (report == null) return NotFound();

            return Ok(new Report
            {
                CanEdit = report.ReporterId == _currentUser.Id.Value,
                Date = report.Date,
                NumberOfWorkers = report.NumberOfWorkers,
                Work = report.Work.Select(w => new ReportQuantity
                {
                    EstimationId = w.EstimationId,
                    Quantity = w.Quantity
                })
            });
        }

        [HttpPut("{id}/report")]
        public IActionResult Put(int id, [FromBody]Report reportDto)
        {
            var buildingId = new BuildingId(id);
            var reportRepository = GetBulidingRepositoryFactory(buildingId).Report;

            var report = new global::BuildingContext.Domain.Report(reportDto.Date, reportDto.NumberOfWorkers);

            foreach (var reportQuantity in reportDto.Work)
            {
                report.AddWork(reportQuantity.EstimationId, reportQuantity.Quantity);
            }
            report.ReportedBy(_currentUser.Id.Value);
            reportRepository.Save(report);
            return Ok();
        }

        [HttpPost("{id}/report/{reportId}")]
        public IActionResult PostReport(int id, int reportId, [FromBody]Report reportDto)
        {
            var buildingId = new BuildingId(id);
            var reportRepository = GetBulidingRepositoryFactory(buildingId).Report;
            
            var report = new global::BuildingContext.Domain.Report(reportId, reportDto.Date, reportDto.NumberOfWorkers);

            foreach (var reportQuantity in reportDto.Work)
            {
                report.AddWork(reportQuantity.EstimationId, reportQuantity.Quantity);
            }

            report.ReportedBy(_currentUser.Id.Value);
            reportRepository.Save(report);
            return Ok();
        }

        [HttpPost("{id}/reports")]
        public IActionResult Reports(int id, [FromBody] DateReport date)
        {
            var buildingId = new BuildingId(id);
            var building = _buildingRepository.Get(buildingId);
            var user = _currentUser.User;
            var userRoles = user.Roles.Where(r => r.BuildingId == buildingId).ToList();

            var reportRepository = GetBulidingRepositoryFactory(buildingId).Report;

            IEnumerable<global::BuildingContext.Repositories.ReportListItem> reports;
            if (user.CompanyId == building.MainContractor.Id && userRoles.Any(r => r.UserBuildingRole == UserBuildingRole.Admin))
                reports = reportRepository.GetAll(date.Month, date.Year);
            else if (userRoles.Any(r => r.UserBuildingRole == UserBuildingRole.Admin))
            {
                var usersId = _userRepository.GetBuildingWorkers(buildingId, user.CompanyId)
                    .Select(u => u.Id);
                reports = reportRepository.GetFor(date.Month, date.Year, usersId.Select(i => i.Value).ToArray());
            }
            else
                reports = reportRepository.GetFor(date.Month, date.Year, user.Id.Value);

            var companies = _companyRepository.GetContractors(buildingId);
            var users = new List<User>();
            foreach (var company in companies)
            {
                users.AddRange(_userRepository.GetWorkers(company.Id));
            }

            return Ok(reports.GroupBy(r => r.Date.Day)
                .Select(g => new ReportList
                {
                    Day = g.Key,
                    Reports = g.Select(r =>
                    {
                        var us = users.Single(u => u.Id.Value == r.UserId);
                        return new ReportListItem
                        {
                            Id = r.Id,
                            CompanyName =
                                companies.Single(c => c.Id == us.CompanyId).Name,
                            UserName = $"{us.FirstName} {us.LastName} ({us.Name})"
                        };
                    })
                }).OrderBy(g => g.Day));
        }

        [HttpPost("{id}/estimation")]
        public IActionResult Post(int id)
        {
            var buildingId = new BuildingId(id);
            var building = _buildingRepository.Get(buildingId);

            var file = Request.Form.Files[0];
            using (var stream = new FileStream($"C:\\Users\\mbube\\Desktop\\Tmp\\{file.FileName}", FileMode.Create))
            {
                file.CopyTo(stream);
                stream.Position = 0;
                var estimationRepository = GetBulidingRepositoryFactory(buildingId).Estimation;
                estimationRepository.Create(stream, building.MainContractor.Id.Value);
            }

            return Ok();
        }

        [HttpPost("{id}/estimation/{estimationId}")]
        public IActionResult Post(int id, int estimationId, [FromBody]Estimation estimation)
        {
            var buildingId = new BuildingId(id);
            var estimationRepository = GetBulidingRepositoryFactory(buildingId).Estimation;
            var est = new global::BuildingContext.Domain.Estimation(
                estimationId,
                estimation.EstimationId,
                estimation.SpecNumber,
                estimation.Description,
                estimation.Unit,
                estimation.Quantity,
                estimation.UnitPrice,
                estimation.Amount,
                estimation.CompanyId);

            estimationRepository.Save(est);

            return Ok();
        }

        [HttpGet("{id}/estimation")]
        public IActionResult GetEstimation(int id)
        {
            var buildingId = new BuildingId(id);

            var estimationRepository = GetBulidingRepositoryFactory(buildingId).Estimation;
            var estimations = estimationRepository.Get();

            return Ok(estimations.Select(e 
                => new Estimation
                {
                    Id = e.Id,
                    EstimationId = e.EstimationId,
                    SpecNumber = e.SpecNumber,
                    Description = e.Description,
                    Unit = e.Unit,
                    Quantity = e.Quantity,
                    UnitPrice = e.UnitPrice,
                    Amount = e.Amount,
                    CompanyId = e.CompanyId
                }));
        }


        [HttpGet("{id}/estimation/{estId}/reports")]
        public IActionResult GetEstimationReport(int id, int estId)
        {
            var buildingId = new BuildingId(id);

            var reportRepository = GetBulidingRepositoryFactory(buildingId).Report;

            var reports = reportRepository.GetFor(estId);

            var companies = _companyRepository.GetContractors(buildingId);
            return Ok(reports.OrderBy(r => r.Date)
                .Select(r =>
            {
                var user = _userRepository.Get(new UserId(r.ReporterId));
                return new
                {
                    CompanyName = companies.First(c => c.Id == user.CompanyId).Name,
                    UserName = $"{user.FirstName} {user.LastName} ({user.Name})",
                    Date = r.Date,
                    Quantity = r.Work[0].Quantity
                };
            }));
        }

        [HttpGet("{id}/companies")]
        public IActionResult GetCompanies(int id)
        {
            var buildingId = new BuildingId(id);
            var building = _buildingRepository.Get(buildingId);
            var companies = building.SubContractors.Union(new [] { building.MainContractor });
            return Ok(companies.Select(c
                => new BuildingCompany
                {
                    Id = c.Id.Value,
                    Name = c.Name,
                    MainContract = building.MainContractor.Id == c.Id,
                    Workers = _userRepository.GetBuildingWorkers(buildingId, c.Id)
                        .Select(u => new BuildingWorker
                        {
                            UserId = u.Id.Value,
                            DisplayName = $"{u.FirstName} {u.LastName} ({u.Name})",
                            UserBuildingRoles = u.Roles.Where(r => r.BuildingId == buildingId).Select(r => r.UserBuildingRole)
                        })
                }));
        }

        [HttpPut("{id}/companies/add")]
        public IActionResult AddNewSub(int id, [FromBody]string email)
        {
            var building = _buildingRepository.Get(new BuildingId(id));
            
            _mailService.SendCompanyInvitedInfo(_currentUser.User, building, email);

            return Ok();
        }

        [HttpPost("{id}/companies/add")]
        public IActionResult AddSub(int id, [FromBody]int companyId)
        {
            _companyRepository.AddSubContract(new BuildingId(id), new CompanyId(companyId));
            return Ok();
        }

        [HttpPost("{id}/worker/add")]
        public IActionResult AddWorker(int id, [FromBody]BuildingWorker worker)
        {
            var buildingId = new BuildingId(id);
            var user = _userRepository.Get(new UserId(worker.UserId));

            foreach (var workerUserBuildingRole in worker.UserBuildingRoles)
            {
                user.AddRole(buildingId, workerUserBuildingRole);
            }

            _userRepository.Save(user);

            return Ok();
        }
    }
    
    public class BuildingCompany
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool MainContract { get; set; }

        public IEnumerable<BuildingWorker> Workers { get; set; }
    }

    public class BuildingWorker
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<UserBuildingRole> UserBuildingRoles { get; set; }
    }

    public class Estimation
    {
        public int Id { get; set; }
        public string EstimationId { get; set; }
        public string SpecNumber { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public int CompanyId { get; set; }
    }

    public class Report
    {
        public bool CanEdit { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfWorkers { get; set; }
        public  IEnumerable<ReportQuantity> Work { get; set; }
    }

    public class ReportQuantity
    {
        public int EstimationId { get; set; }
        public decimal Quantity { get; set; }
    }

    public class ReportList
    {
        public int Day { get; set; }
        public IEnumerable<ReportListItem> Reports { get; set; }
    }

    public class ReportListItem
    {
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public object Id { get; set; }
    }

    public class DateReport
    {
        public int Year { get; set; }
        public int Month { get; set; }
    }

    public class DateRange
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
