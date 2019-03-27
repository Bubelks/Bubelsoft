namespace BubelSoft.Building.Infrastructure.Entities
{
    public class ReportQuantity
    {
        public Estimation Estimation { get; set; }
        public int EstimationId { get; set; }
        public decimal Quantity { get; set; }
        public int ReportId { get; set; }
        public Report Report { get; set; }
    }
}
