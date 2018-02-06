using System;
using System.Collections.Generic;

namespace BuildingContext.Domain
{
    public class Report
    {
        public DateTime Date { get; }
        public int NumberOfWorkers { get; }
        public IList<ReportQuantity> Work { get; }

        public Report(DateTime date, int numberOfWorkers)
        {
            Date = date;
            NumberOfWorkers = numberOfWorkers;
            Work = new List<ReportQuantity>();
        }

        public Report(int id, DateTime date, int numberOfWorkers): this(date, numberOfWorkers)
        {
            Id = id;
        }

        public void AddWork(int estimationId, decimal quantity)
        {
            Work.Add(new ReportQuantity(estimationId, quantity));
        }

        public void ReportedBy(int userId)
        {
            ReporterId = userId;
        }

        public int ReporterId { get; private set; }
        public int Id { get; }
    }

    public class ReportQuantity
    {
        public int EstimationId { get; }
        public decimal Quantity { get; }

        public ReportQuantity(int estimationId, decimal quantity)
        {
            EstimationId = estimationId;
            Quantity = quantity;
        }
    }
}