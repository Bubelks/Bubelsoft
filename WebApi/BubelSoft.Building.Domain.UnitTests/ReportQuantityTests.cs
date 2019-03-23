using System;
using BubelSoft.Building.Domain.Models;
using NUnit.Framework;

namespace BubelSoft.Building.Domain.UnitTests
{
    [TestFixture]
    public class ReportQuantityTests
    {
        [TestCase(0)]
        [TestCase(-5)]
        public void Quantity_ShouldBeGreaterThan0(decimal quantity)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ReportQuantity(1, quantity));
        }
    }
}