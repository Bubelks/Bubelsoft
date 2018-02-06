using System;
using System.Collections.Generic;
using System.Linq;
using BuildingContext.Domain;
using Microsoft.EntityFrameworkCore;
using ReportQuantity = BuildingContext.Entities.ReportQuantity;

namespace BuildingContext.Repositories
{
    internal class ReportRepository : IReportRepository
    {
        private readonly BuildingContext _buildingContext;

        public ReportRepository(BuildingContext buildingContext)
        {
            _buildingContext = buildingContext;
        }

        public void Save(Report report)
        {
            var entity = _buildingContext.Reports.SingleOrDefault(r => r.Id == report.Id) ?? new Entities.Report();

            entity.CreatedDate = DateTime.UtcNow;
            entity.ReportDate = report.Date;
            entity.WorkersCount = report.NumberOfWorkers;
            entity.ReporterId = report.ReporterId;
            entity.Quantities = report.Work.Select(w => new ReportQuantity
            {
                EstimationId = w.EstimationId,
                Quantity = w.Quantity
            }).ToList();

            if(report.Id == 0)
                _buildingContext.Reports.Add(entity);

            _buildingContext.SaveChanges();
        }

        public IEnumerable<ReportListItem> GetAll(int month, int year)
        {
            return _buildingContext.Reports
                .Where(r => r.ReportDate.Year == year && r.ReportDate.Month == month)
                .Include(r => r.Quantities)
                .ToList()
                .Select(r => new ReportListItem
                {
                    Id = r.Id,
                    Date = r.ReportDate,
                    UserId = r.ReporterId
                });
        }

        public IEnumerable<ReportListItem> GetFor(int month, int year, params int[] usersId)
        {
            return _buildingContext.Reports
                .Where(r => r.ReportDate.Year == year && r.ReportDate.Month == month)
                .Where(r => usersId.Contains(r.ReporterId))
                .Include(r => r.Quantities)
                .ToList()
                .Select(r => new ReportListItem
                {
                    Id = r.Id,
                    Date = r.ReportDate,
                    UserId = r.ReporterId
                });
        }

        public Report Get(int reportId)
        {
            var entity = _buildingContext.Reports.Include(r => r.Quantities).SingleOrDefault(r => r.Id == reportId);

            if (entity == null) return null;

            var report = new Report(entity.ReportDate, entity.WorkersCount);

            report.ReportedBy(entity.ReporterId);
            foreach (var entityQuantity in entity.Quantities)
            {
                report.AddWork(entityQuantity.EstimationId, entityQuantity.Quantity);
            }

            return report;
        }

        public IEnumerable<Report> GetFor(int estId)
        {
            return _buildingContext.Reports
                .Where(r => r.Quantities.Any(q => q.EstimationId == estId))
                .Include(r => r.Quantities)
                .ToList()
                .Select(r =>
                {
                    var report = new Report(r.Id, r.ReportDate, r.WorkersCount);
                    report.ReportedBy(r.ReporterId);
                    var estQuantity = r.Quantities.Single(q => q.EstimationId == estId);
                    report.AddWork(estId, estQuantity.Quantity);
                    return report;
                });
        }
    }

    public interface IReportRepository
    {
        void Save(Report report);
        IEnumerable<ReportListItem> GetAll(int month, int year);
        IEnumerable<ReportListItem> GetFor(int month, int year, params int[] usersId);
        Report Get(int reportId);
        IEnumerable<Report> GetFor(int estId);
    }
    
    public class ReportListItem
    {
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public object Id { get; set; }
    }
}