using System;
using BubelSoft.Building.Domain.Models;
using BubelSoft.Core.Domain.Models;
using NUnit.Framework;

namespace BubelSoft.Building.Domain.UnitTests
{
    [TestFixture]
    public class EstimationTests
    {
        [Test]
        public void New_ReturnWithIdEqual0()
        {
            var estimation = Estimation.New("id", "specNumber", "Description", "unit", 1, 1, 1, new CompanyId(1));

            Assert.That(estimation.Id, Is.Zero, $"Id should be 0, but is {estimation.Id}");
        }

        [Test]
        public void Exsiting_GivenIdEqual7_ReturnWithIdEqual7()
        {
            var givenId = 7;

            var estimation = Estimation.Existing(givenId, "id", "specNumber", "Description", "unit", 1, 1, 1, new CompanyId(1));

            Assert.That(estimation.Id, Is.EqualTo(givenId), $"Id should be {givenId}, but is {estimation.Id}");
        }

        [TestCase(-1, 1, 1)]
        [TestCase(1, -1, 1)]
        [TestCase(1, 1, -1)]
        [TestCase(0, 1, 1)]
        [TestCase(1, 0, 1)]
        [TestCase(1, 1, 0)]
        public void QuantityUnitPriceAmount_ShouldBeGreaterThen0(decimal quantity, decimal unitPrice, decimal amount)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Estimation.New("id", "specNumber", "Description", "unit", quantity, unitPrice, amount, new CompanyId(1)));
        }
    }
}
