using System;
using System.Reflection.Metadata.Ecma335;
using BubelSoft.Core.Domain.Models;

namespace BubelSoft.Building.Domain.Models
{
    public class Estimation
    {
        private Estimation(int id, string estimationId, string specNumber, string description, string unit, decimal quantity, decimal unitPrice, decimal amount, CompanyId companyId)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            if (unitPrice <= 0)
                throw new ArgumentOutOfRangeException(nameof(unitPrice));

            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

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

        public static Estimation New(string estimationId, string specNumber, string description, string unit, decimal quantity,
            decimal unitPrice, decimal amount, CompanyId companyId)
        {
            return new Estimation(0, estimationId, specNumber, description, unit, quantity, unitPrice, amount, companyId);
        }

        public static Estimation Existing(int id, string estimationId, string specNumber, string description, string unit,
            decimal quantity,
            decimal unitPrice, decimal amount, CompanyId companyId)
        {
            return new Estimation(id, estimationId, specNumber, description, unit, quantity, unitPrice, amount, companyId);
        }

        public int Id { get; }

        public string EstimationId { get; }

        public string SpecNumber { get; }

        public string Description { get; }

        public string Unit { get; }

        public decimal Quantity { get; }

        public decimal UnitPrice { get; }

        public decimal Amount { get; }
        public CompanyId CompanyId { get; }
    }
}