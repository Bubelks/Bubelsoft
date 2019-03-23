using System;
using BubelSoft.Building.Domain.Models;
using BubelSoft.Core.Domain.Models;
using NUnit.Framework;

namespace BubelSoft.Building.Domain.UnitTests
{
    [TestFixture]
    public class ReportTests
    {
        [Test]
        public void ReportDate_ShouldBeTodayOrPast()
        {
            var mostNowInWorld = DateTime.UtcNow.AddHours(12);
            var futureDate = mostNowInWorld.AddDays(1);

            Assert.Throws<ArgumentOutOfRangeException>(() => new Report(futureDate, 1, new UserId(1), new BuildingId(2)));
        }

        [TestCase(0)]
        [TestCase(-2)]
        public void ReportDate_ShouldBeGreaterThen0(int numberOfWorkers)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Report(DateTime.UtcNow, numberOfWorkers, new UserId(1), new BuildingId(2)));
        }

        [Test]
        public void AddOrUpdatework_ShouldAddIfNotExistsAndUpdateIfEsists()
        {
            const int estId = 3;
            var firstQuantity = new decimal(2.4);
            var report = new Report(DateTime.Now, 1, new UserId(1), new BuildingId(2));

            report.AddOrUpdateWork(estId, firstQuantity);

            Assert.That(report.Work, Has.Count.EqualTo(1), $"Report should have 1 work, but have {report.Work.Count}");
            Assert.That(report.Work[0].EstimationId, Is.EqualTo(estId));
            Assert.That(report.Work[0].Quantity, Is.EqualTo(firstQuantity));
            
            var newQuantity = new decimal(5.5);
            report.AddOrUpdateWork(estId, newQuantity);

            Assert.That(report.Work, Has.Count.EqualTo(1), $"Report should have 1 work, but have {report.Work.Count}");
            Assert.That(report.Work[0].EstimationId, Is.EqualTo(estId));
            Assert.That(report.Work[0].Quantity, Is.EqualTo(newQuantity), $"Report work should have updated quantity to {newQuantity}, but have {report.Work[0].Quantity}");
        }
    }
}