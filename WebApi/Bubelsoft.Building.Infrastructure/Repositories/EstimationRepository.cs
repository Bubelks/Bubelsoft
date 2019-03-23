using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bubelsoft.Building.Infrastructure.Entities;
using BubelSoft.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using NPOI.XSSF.UserModel;

namespace Bubelsoft.Building.Infrastructure.Repositories
{
    internal class EstimationRepository : IEstimationRepository
    {
        private readonly BuildingContext _buildingContext;

        public EstimationRepository(BuildingContext buildingContext)
        {
            _buildingContext = buildingContext;
        }

        public void Create(FileStream stream, int mainContractorId)
        {
            var hssfwb = new XSSFWorkbook(stream);
            var sheet = hssfwb.GetSheetAt(0);
            for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                var entity = new Estimation
                {
                    EstimationId = row.GetCell(0).ToString(),
                    SpecNumber = row.GetCell(1).ToString(),
                    Description = row.GetCell(2).ToString(),
                    Unit = row.GetCell(3).ToString(),
                    Quantity = (decimal) row.GetCell(4).NumericCellValue,
                    UnitPrice = (decimal) row.GetCell(5).NumericCellValue,
                    Amount = (decimal) row.GetCell(6).NumericCellValue,
                    CompanyId = mainContractorId
                };
                _buildingContext.Estimations.Add(entity);
            }

            _buildingContext.SaveChanges();
        }

        public IEnumerable<BubelSoft.Building.Domain.Models.Estimation> Get(int skip, int take)
        {
            return _buildingContext.Estimations.Skip(skip).Take(take)
                .Select(e
                => BubelSoft.Building.Domain.Models.Estimation.Existing(
                    e.Id,
                    e.EstimationId,
                    e.SpecNumber,
                    e.Description,
                    e.Unit,
                    e.Quantity,
                    e.UnitPrice,
                    e.Amount,
                    new CompanyId(e.CompanyId)));
        }

        public void Save(BubelSoft.Building.Domain.Models.Estimation est)
        {
            var entity = _buildingContext.Estimations.Single(e => e.Id == est.Id);

            entity.CompanyId = est.CompanyId.Value;
            entity.EstimationId = est.EstimationId;
            entity.SpecNumber = est.SpecNumber;
            entity.Description = est.Description;
            entity.UnitPrice = est.UnitPrice;
            entity.Unit = est.Unit;
            entity.Quantity = est.Quantity;
            entity.Amount = est.Amount;

            _buildingContext.SaveChanges();
        }

        public IEnumerable<EstimationReportList> GetAllReported(int skip, int take)
        {
            var reports = _buildingContext.Reports
                .Include(r => r.Quantities)
                .ThenInclude(q => q.Estimation);

            var estimations = reports.SelectMany(r => r.Quantities).Select(q => q.Estimation).Distinct().ToList().Skip(skip).Take(take);

            return estimations.Select(e => new EstimationReportList
            {
                Estimation = BubelSoft.Building.Domain.Models.Estimation.Existing(e.Id, e.EstimationId, e.SpecNumber, e.Description, e.Unit,
                    e.Quantity, e.UnitPrice, e.Amount, new CompanyId(e.CompanyId)),
                Reports = reports.Where(r => r.Quantities.Any(q => q.EstimationId == e.Id)).Select(r =>
                    new EstimationReport
                    {
                        ReporterId = r.ReporterId,
                        Date = r.ReportDate,
                        Quantity = r.Quantities.Single(q => q.EstimationId == e.Id).Quantity
                    })
            });
        }

        public IEnumerable<EstimationReportList> GetFrom(DateTime dateRangeFrom, DateTime dateRangeTo, int skip, int take)
        {
            var reports = _buildingContext.Reports
                .Where(r => r.ReportDate >= dateRangeFrom && r.ReportDate <= dateRangeTo)
                .Include(r => r.Quantities)
                .ThenInclude(q => q.Estimation);

            var estimations = reports.SelectMany(r => r.Quantities).Select(q => q.Estimation).Distinct().ToList().Skip(skip).Take(take);

            return estimations.Select(e => new EstimationReportList
            {
                Estimation = BubelSoft.Building.Domain.Models.Estimation.Existing(e.Id, e.EstimationId, e.SpecNumber, e.Description, e.Unit,
                    e.Quantity, e.UnitPrice, e.Amount, new CompanyId(e.CompanyId)),
                Reports = reports.Where(r => r.Quantities.Any(q => q.EstimationId == e.Id)).Select(r =>
                    new EstimationReport
                    {
                        ReporterId = r.ReporterId,
                        Date = r.ReportDate,
                        Quantity = r.Quantities.Single(q => q.EstimationId == e.Id).Quantity
                    })
            });
        }
        public int CountFrom(DateTime dateRangeFrom, DateTime dateRangeTo)
        {
            var reports = _buildingContext.Reports
                .Where(r => r.ReportDate >= dateRangeFrom && r.ReportDate <= dateRangeTo)
                .Include(r => r.Quantities)
                .ThenInclude(q => q.Estimation);

            var estimations = reports.SelectMany(r => r.Quantities).Select(q => q.Estimation).Distinct();

            return estimations.Count();
        }

        public int CountAllReported()
        {
            var reports = _buildingContext.Reports
                .Include(r => r.Quantities)
                .ThenInclude(q => q.Estimation);

            return reports.SelectMany(r => r.Quantities).Select(q => q.Estimation).Distinct().ToList().Count();

        }

        public bool Exists(int estimationId)
        {
            return _buildingContext.Estimations.Any(e => e.Id == estimationId);
        }

        public int Count()
        {
            return _buildingContext.Estimations.Count();
        }
    }

    public interface IEstimationRepository
    {
        void Create(FileStream stream, int mainContractorId);
        IEnumerable<BubelSoft.Building.Domain.Models.Estimation> Get(int skip, int take);
        void Save(BubelSoft.Building.Domain.Models.Estimation est);
        IEnumerable<EstimationReportList> GetAllReported(int skip, int take);
        IEnumerable<EstimationReportList> GetFrom(DateTime dateRangeFrom, DateTime dateRangeTo, int skip, int take);
        bool Exists(int estimationId);
        int Count();
        int CountFrom(DateTime dateRangeFrom, DateTime dateRangeTo);
        int CountAllReported();
    }

    public class EstimationReportList
    {
        public BubelSoft.Building.Domain.Models.Estimation Estimation { get; set; }
        public IEnumerable<EstimationReport> Reports { get; set; }
    }

    public class EstimationReport
    {
        public int ReporterId { get; set; }
        public DateTime Date { get; set; }
        public decimal Quantity { get; set; }
    }
}