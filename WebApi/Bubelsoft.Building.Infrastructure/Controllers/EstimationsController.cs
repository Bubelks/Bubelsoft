using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BubelSoft.Building.Domain.AccessRules;
using BubelSoft.Building.Infrastructure.Repositories;
using BubelSoft.Core.Domain.Models;
using BubelSoft.Core.Infrastructure.Database.Repositories.Interfaces;
using BubelSoft.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BubelSoft.Building.Infrastructure.Controllers
{
    [Authorize]
    [Route("api/buildings/{id}/estimations")]
    public class EstimationsController: BuildingContextController
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserSession _userSession;
        private readonly IEstimationAccessRules _estimationAccessRules;

        public EstimationsController(IBuildingRepository buildingRepository,
            ICompanyRepository companyRepository,
            IUserRepository userRepository,
            IUserSession userSession,
            IEstimationAccessRules estimationAccessRules,
            IRepositoryFactory repositoryFactory) : base(buildingRepository, repositoryFactory)
        {
            _buildingRepository = buildingRepository;
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _userSession = userSession;
            _estimationAccessRules = estimationAccessRules;
        }

        [HttpPost("mainReports")]
        public IActionResult GetMainReport(int id, [FromBody] DateRange dateRange, [FromQuery]int skip, [FromQuery]int take)
        {
            var buildingId = new BuildingId(id);
            var estimationRepository = EstimationRepository(buildingId);

            IEnumerable<EstimationReportList> reports;
            int count;
            if (dateRange.From.Year == 1 || dateRange.To.Year == 1)
            {
                reports = estimationRepository.GetAllReported(skip, take);
                count = estimationRepository.CountAllReported();
            }
            else
            {
                reports = estimationRepository.GetFrom(dateRange.From, dateRange.To, skip, take);
                count = estimationRepository.CountFrom(dateRange.From, dateRange.To);
            }

            var result = new {
                Count = count,
                Reports = reports.Select(r => new
                {
                    Id = r.Estimation.Id,
                    CompanyId = r.Estimation.CompanyId,
                    EstimationId = r.Estimation.EstimationId,
                    SpecNumber = r.Estimation.SpecNumber,
                    Description = r.Estimation.Description,
                    Unit = r.Estimation.Unit,
                    Quantity = r.Estimation.Quantity,
                    UnitPrice = r.Estimation.UnitPrice,
                    Amount = r.Estimation.Amount,
                    DoneQuantity = r.Reports.Sum(rq => rq.Quantity)
                })
            };

            return Ok(result);
        }

        

        [HttpPost]
        public IActionResult Create(int id)
        {
            var buildingId = new BuildingId(id);
            var building = _buildingRepository.Get(buildingId);

            var file = Request.Form.Files[0];
            using (var stream = new FileStream($"C:\\Users\\mbube\\Desktop\\Tmp\\{file.FileName}", FileMode.Create))
            {
                file.CopyTo(stream);
                stream.Position = 0;
                var estimationRepository = EstimationRepository(buildingId);
                estimationRepository.Create(stream, building.MainContractor.Id.Value);
            }

            return Ok();
        }

        [HttpPost("{estimationId}")]
        public IActionResult Update(int id, int estimationId, [FromBody]Estimation estimation)
        {
            var buildingId = new BuildingId(id);
            var estimationRepository = EstimationRepository(buildingId);

            if (!estimationRepository.Exists(estimationId))
                return NotFound();
            
            var est = BubelSoft.Building.Domain.Models.Estimation.Existing(
                estimationId,
                estimation.EstimationId,
                estimation.SpecNumber,
                estimation.Description,
                estimation.Unit,
                estimation.Quantity,
                estimation.UnitPrice,
                estimation.Amount,
                new CompanyId(estimation.CompanyId));

            if (!_estimationAccessRules.CanEdit(est, _userSession.User.Id, buildingId))
                return Forbid();

            estimationRepository.Save(est);

            return Ok();
        }

        [HttpGet]
        public IActionResult GetEstimations(int id, [FromQuery]int skip, [FromQuery]int take)
        {
            var buildingId = new BuildingId(id);

            var estimationRepository = EstimationRepository(buildingId);
            var estimations = estimationRepository.Get(skip, take);
            var count = estimationRepository.Count();

            return Ok(new {
                Count = count,
                Estimations = estimations.Select(e 
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
                    CompanyId = e.CompanyId.Value
                })
            });
        }


        [HttpGet("{estId}/reports")]
        public IActionResult GetEstimationReport(int id, int estId)
        {
            var buildingId = new BuildingId(id);

            var estimationRepository = EstimationRepository(buildingId);

            if (!estimationRepository.Exists(estId))
                return NotFound();

            var reportRepository = ReportRepository(buildingId);
            var reports = reportRepository.GetFor(estId, buildingId);

            var companies = _companyRepository.GetContractors(buildingId);
            return Ok(reports.OrderBy(r => r.Date)
                .Select(r =>
            {
                var user = _userRepository.Get(r.ReporterId);
                return new
                {
                    CompanyName = companies.First(c => c.Id == user.CompanyId).Name,
                    UserName = $"{user.FirstName} {user.LastName} ({user.Name})",
                    Date = r.Date,
                    Quantity = r.Work[0].Quantity
                };
            }));
        }
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

    public class DateRange
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
