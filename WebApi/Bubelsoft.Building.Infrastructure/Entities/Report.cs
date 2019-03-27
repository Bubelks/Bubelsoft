using System;
using System.Collections.Generic;

namespace BubelSoft.Building.Infrastructure.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public int ReporterId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ReportDate { get; set; }
        public int WorkersCount { get; set; }
        public List<ReportQuantity> Quantities { get; set; }
    }
}
