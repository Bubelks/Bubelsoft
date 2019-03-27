using System;
using System.Collections.Generic;
using System.Linq;
using BubelSoft.Building.Domain.AccessRules;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using BubelSoft.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BubelSoft.Building.Infrastructure.Controllers
{
    [Authorize]
    [Route("api/buildings/{id}/reports")]
    public class ReportsController : BuildingContextController
    {
        private readonly IUserSession _userSession;
        private readonly IUserRepository _userRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IReportAccessRules _reportAccessRules;

        public ReportsController(
            IBuildingRepository buildingRepository,
            IUserSession userSession, 
            IUserRepository userRepository, 
            ICompanyRepository companyRepository,
            IRepositoryFactory repositoryFactory,
            IReportAccessRules reportAccessRules) 
            : base(buildingRepository, repositoryFactory)
        {
            _userSession = userSession;
            _userRepository = userRepository;
            _companyRepository = companyRepository;
            _reportAccessRules = reportAccessRules;
        }

        [HttpGet("{reportId}")]
        public IActionResult GetReport(int id, int reportId)
        {
            var buildingId = new BuildingId(id);
            var reportRepository = ReportRepository(buildingId);

            var report = reportRepository.Get(reportId, buildingId);

            if (report == null) return NotFound();

            if (!_reportAccessRules.CanAccess(report, _userSession.User.Id)) return Forbid();

            return Ok(new ReportDTO
            {
                CanEdit = report.ReporterId == _userSession.User.Id,
                Date = report.Date,
                NumberOfWorkers = report.NumberOfWorkers,
                Work = report.Work.Select(w => new ReportQuantity
                {
                    EstimationId = w.EstimationId,
                    Quantity = w.Quantity
                })
            });
        }

        [HttpPut]
        public IActionResult CreateReport(int id, [FromBody]ReportDTO reportDtoDto)
        {
            var buildingId = new BuildingId(id);
            var reportRepository = ReportRepository(buildingId);

            var report = new BubelSoft.Building.Domain.Models.Report(reportDtoDto.Date, reportDtoDto.NumberOfWorkers, _userSession.User.Id, buildingId);

            foreach (var reportQuantity in reportDtoDto.Work)
            {
                report.AddOrUpdateWork(reportQuantity.EstimationId, reportQuantity.Quantity);
            }

            reportRepository.Save(report);
            return Ok(report.Id);
        }

        [HttpPost("{reportId}")]
        public IActionResult UpdateReport(int id, int reportId, [FromBody]ReportDTO reportDtoDto)
        {
            var buildingId = new BuildingId(id);
            var reportRepository = ReportRepository(buildingId);

            if (!reportRepository.Exists(reportId))
                return NotFound();

            var report = new BubelSoft.Building.Domain.Models.Report(reportId, reportDtoDto.Date, reportDtoDto.NumberOfWorkers, _userSession.User.Id, buildingId);

            if (!_reportAccessRules.CanAccess(report, _userSession.User.Id)) return Forbid();

            foreach (var reportQuantity in reportDtoDto.Work)
            {
                report.AddOrUpdateWork(reportQuantity.EstimationId, reportQuantity.Quantity);
            }

            reportRepository.Save(report);
            return Ok();
        }

        [HttpPost]
        public IActionResult GetReports(int id, [FromBody] ReportDate reportDate)
        {
            var buildingId = new BuildingId(id);
            var building = BuildingRepository.Get(buildingId);
            var user = _userSession.User;
            var userRoles = user.Roles.Where(r => r.BuildingId == buildingId).ToList();

            var reportRepository = ReportRepository(buildingId);

            IEnumerable<Repositories.ReportListItem> reports;
            if (user.CompanyId == building.MainContractor.Id && userRoles.Any(r => r.UserBuildingRole == UserBuildingRole.Admin))
                reports = reportRepository.GetAll(reportDate.Month, reportDate.Year);
            else if (userRoles.Any(r => r.UserBuildingRole == UserBuildingRole.Admin))
            {
                var usersId = _userRepository.GetBuildingWorkers(buildingId, user.CompanyId)
                    .Select(u => u.Id);
                reports = reportRepository.GetFor(reportDate.Month, reportDate.Year, usersId.Select(i => i.Value).ToArray());
            }
            else
                reports = reportRepository.GetFor(reportDate.Month, reportDate.Year, user.Id.Value);

            var companies = _companyRepository.GetContractors(buildingId).ToList();
            var users = companies.SelectMany(c => _userRepository.GetWorkers(c.Id));

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
    }

    public class ReportDTO
    {
        public bool CanEdit { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfWorkers { get; set; }
        public IEnumerable<ReportQuantity> Work { get; set; }
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

    public class ReportDate
    {
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
