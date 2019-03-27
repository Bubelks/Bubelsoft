using System;

namespace BubelSoft.Building.Domain.Models
{
    public class ReportQuantity
    {
        public int EstimationId { get; }
        public decimal Quantity { get; private set; }

        public ReportQuantity(int estimationId, decimal quantity)
        {
            UpdateQuantity(quantity);
            EstimationId = estimationId;
        }

        public void UpdateQuantity(decimal quantity)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            Quantity = quantity;
        }
    }
}