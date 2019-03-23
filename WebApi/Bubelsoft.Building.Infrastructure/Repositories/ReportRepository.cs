using System;
using System.Collections.Generic;
using System.Linq;
using BubelSoft.Building.Domain.Models;
using BubelSoft.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using ReportQuantity = Bubelsoft.Building.Infrastructure.Entities.ReportQuantity;

namespace Bubelsoft.Building.Infrastructure.Repositories
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
            var entity = _buildingContext.Reports.Include(r => r.Quantities).ToList().SingleOrDefault(r => r.Id == report.Id) ?? new Entities.Report();

            entity.CreatedDate = DateTime.UtcNow;
            entity.ReportDate = report.Date;
            entity.WorkersCount = report.NumberOfWorkers;
            entity.ReporterId = report.ReporterId.Value;

            if (entity.Quantities != null)
            {
                entity.Quantities.RemoveAll(q => report.Work.All(w => w.EstimationId != q.EstimationId));
                foreach (var work in report.Work)
                {
                    var quantity = entity.Quantities.SingleOrDefault(q => q.EstimationId == work.EstimationId);
                    if (quantity != null)
                        quantity.Quantity = work.Quantity;
                    else
                        entity.Quantities.Add(new ReportQuantity
                        {
                            EstimationId = work.EstimationId,
                            Quantity = work.Quantity
                        });
                }
            }
            else
                entity.Quantities = report.Work.Select(w => new ReportQuantity
                {
                    EstimationId = w.EstimationId,
                    Quantity = w.Quantity
                }).ToList();

            if(report.Id == 0)
                _buildingContext.Reports.Add(entity);

            _buildingContext.SaveChanges();

            if (report.Id == 0)
                report.SetId(entity.Id);
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

        public Report Get(int reportId, BuildingId buildingId)
        {
            var entity = _buildingContext.Reports.Include(r => r.Quantities).SingleOrDefault(r => r.Id == reportId);

            if (entity == null) return null;

            var report = new Report(entity.ReportDate, entity.WorkersCount, new UserId(entity.ReporterId), buildingId);

            foreach (var entityQuantity in entity.Quantities)
            {
                report.AddOrUpdateWork(entityQuantity.EstimationId, entityQuantity.Quantity);
            }

            return report;
        }

        public IEnumerable<Report> GetFor(int estId, BuildingId buildingId)
        {
            return _buildingContext.Reports
                .Where(r => r.Quantities.Any(q => q.EstimationId == estId))
                .Include(r => r.Quantities)
                .ToList()
                .Select(r =>
                {
                    var report = new Report(r.Id, r.ReportDate, r.WorkersCount, new UserId(r.ReporterId), buildingId);
                    var estQuantity = r.Quantities.Single(q => q.EstimationId == estId);
                    report.AddOrUpdateWork(estId, estQuantity.Quantity);
                    return report;
                });
        }

        public bool Exists(int reportId)
        {
            return _buildingContext.Reports.Any(r => r.Id == reportId);
        }
    }

    public interface IReportRepository
    {
        void Save(Report report);
        IEnumerable<ReportListItem> GetAll(int month, int year);
        IEnumerable<ReportListItem> GetFor(int month, int year, params int[] usersId);
        Report Get(int reportId, BuildingId buildingId);
        IEnumerable<Report> GetFor(int estId, BuildingId buildingId);
        bool Exists(int reportId);
    }
    
    public class ReportListItem
    {
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public object Id { get; set; }
    }
}