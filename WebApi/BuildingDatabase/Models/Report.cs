using System;
using System.Collections.Generic;
using System.Linq;
using BubelSoft.Core.Domain.Models;

namespace BubelSoft.Building.Domain.Models
{
    public class Report
    {
        public UserId ReporterId { get; }
        public BuildingId BuildingId { get; }
        public int Id { get; private set; }
        public DateTime Date { get; }
        public int NumberOfWorkers { get; }
        public IList<ReportQuantity> Work { get; }

        public Report(DateTime date, int numberOfWorkers, UserId reporterId, BuildingId buildingId)
        {
            if(numberOfWorkers <= 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfWorkers));

            var mostNowInWorld = DateTime.UtcNow.AddHours(12);
            if(date.Date > mostNowInWorld.Date)
                throw new ArgumentOutOfRangeException(nameof(date));

            Date = date;
            NumberOfWorkers = numberOfWorkers;
            Work = new List<ReportQuantity>();
            ReporterId = reporterId;
            BuildingId = buildingId;
        }

        public Report(int id, DateTime date, int numberOfWorkers, UserId reporterId, BuildingId buildingId) : this(date, numberOfWorkers, reporterId, buildingId)
        {
            Id = id;
        }

        public void AddOrUpdateWork(int estimationId, decimal quantity)
        {
            var existing = Work.SingleOrDefault(w => w.EstimationId == estimationId);
            if (existing != null)
                existing.UpdateQuantity(quantity);
            else
                Work.Add(new ReportQuantity(estimationId, quantity));
        }
        
        public void SetId(int id)
        {
            if (Id !=  0)
                throw new InvalidOperationException("Id is already set");

            Id = id;
        }
    }
}