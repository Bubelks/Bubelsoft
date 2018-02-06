using System;
using System.IO;
using BuildingContext.Entities;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BuildingContext.Repositories
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

        public IEnumerable<Domain.Estimation> Get()
        {
            return _buildingContext.Estimations.Select(e
                => new Domain.Estimation(
                    e.Id,
                    e.EstimationId,
                    e.SpecNumber,
                    e.Description,
                    e.Unit,
                    e.Quantity,
                    e.UnitPrice,
                    e.Amount,
                    e.CompanyId));
        }

        public void Save(Domain.Estimation est)
        {
            var entity = _buildingContext.Estimations.Single(e => e.Id == est.Id);

            entity.CompanyId = est.CompanyId;
            entity.EstimationId = est.EstimationId;
            entity.SpecNumber = est.SpecNumber;
            entity.Description = est.Description;
            entity.UnitPrice = est.UnitPrice;
            entity.Unit = est.Unit;
            entity.Quantity = est.Quantity;
            entity.Amount = est.Amount;

            _buildingContext.SaveChanges();
        }

        public IEnumerable<EstimationReportList> GetAll()
        {
            var reports = _buildingContext.Reports
                .Include(r => r.Quantities)
                .ThenInclude(q => q.Estimation);

            var estimations = reports.SelectMany(r => r.Quantities).Select(q => q.Estimation).Distinct();

            return estimations.Select(e => new EstimationReportList
            {
                Estimation = new Domain.Estimation(e.Id, e.EstimationId, e.SpecNumber, e.Description, e.Unit,
                    e.Quantity, e.UnitPrice, e.Amount, e.CompanyId),
                Reports = reports.Where(r => r.Quantities.Any(q => q.EstimationId == e.Id)).Select(r =>
                    new EstimationReport
                    {
                        ReporterId = r.ReporterId,
                        Date = r.ReportDate,
                        Quantity = r.Quantities.Single(q => q.EstimationId == e.Id).Quantity
                    })
            });
        }

        public IEnumerable<EstimationReportList> GetFrom(DateTime dateRangeFrom, DateTime dateRangeTo)
        {
            var reports = _buildingContext.Reports
                .Where(r => r.ReportDate >= dateRangeFrom && r.ReportDate <= dateRangeTo)
                .Include(r => r.Quantities)
                .ThenInclude(q => q.Estimation);

            var estimations = reports.SelectMany(r => r.Quantities).Select(q => q.Estimation).Distinct();

            return estimations.Select(e => new EstimationReportList
            {
                Estimation = new Domain.Estimation(e.Id, e.EstimationId, e.SpecNumber, e.Description, e.Unit,
                    e.Quantity, e.UnitPrice, e.Amount, e.CompanyId),
                Reports = reports.Where(r => r.Quantities.Any(q => q.EstimationId == e.Id)).Select(r =>
                    new EstimationReport
                    {
                        ReporterId = r.ReporterId,
                        Date = r.ReportDate,
                        Quantity = r.Quantities.Single(q => q.EstimationId == e.Id).Quantity
                    })
            });
        }
    }

    public interface IEstimationRepository
    {
        void Create(FileStream stream, int mainContractorId);
        IEnumerable<Domain.Estimation> Get();
        void Save(Domain.Estimation est);
        IEnumerable<EstimationReportList> GetAll();
        IEnumerable<EstimationReportList> GetFrom(DateTime dateRangeFrom, DateTime dateRangeTo);
    }

    public class EstimationReportList
    {
        public Domain.Estimation Estimation { get; set; }
        public IEnumerable<EstimationReport> Reports { get; set; }
    }

    public class EstimationReport
    {
        public int ReporterId { get; set; }
        public DateTime Date { get; set; }
        public decimal Quantity { get; set; }
    }
}