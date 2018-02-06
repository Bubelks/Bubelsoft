namespace BuildingContext.Domain
{
    public class Estimation
    {
        public Estimation(int id, string estimationId, string specNumber, string description, string unit, decimal quantity, decimal unitPrice, decimal amount, int companyId)
        {
            Id = id;
            EstimationId = estimationId;
            SpecNumber = specNumber;
            Description = description;
            Unit = unit;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Amount = amount;
            CompanyId = companyId;
        }

        public int Id { get; }

        public string EstimationId { get; }

        public string SpecNumber { get; }

        public string Description { get; }

        public string Unit { get; }

        public decimal Quantity { get; }

        public decimal UnitPrice { get; }

        public decimal Amount { get; }
        public int CompanyId { get; }
    }
}