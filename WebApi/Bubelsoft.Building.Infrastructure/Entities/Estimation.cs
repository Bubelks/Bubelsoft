using System.Collections.Generic;

namespace Bubelsoft.Building.Infrastructure.Entities
{
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

        public List<ReportQuantity> Quantities { get; set; }
        public int CompanyId { get; set; }
    }
}
