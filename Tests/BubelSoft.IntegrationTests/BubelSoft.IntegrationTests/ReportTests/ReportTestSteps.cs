using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace BubelSoft.IntegrationTests.ReportTests
{
    [Binding]
    public class ReportTestSteps
    {
        private const string ReportId = "ReportId";
        private const string EstimationId = "EstimationId";
        private const string ReportDate = "ReportDate";
        private const string BuildingId = "buildingId";

        private static RestClient Client => (RestClient)ScenarioContext.Current["Client"];

        [Given(@"Building have estimation")]
        public void GivenBuildingHaveEstimation()
        {
            var buildingId = ScenarioContext.Current[BuildingId];

            var estimations =
                Client.Get<IEnumerable<Estimation>>($"buildings/{buildingId}/estimations").ToList();

            ScenarioContext.Current[EstimationId] = estimations[0].Id;
        }

        [When(@"I create Report")]
        public void WhenICreateReport()
        {
            var report = new ReportDTO
            {
                Date = DateTime.UtcNow,
                NumberOfWorkers = 14,
                Work = new []{
                    new ReportQuantity
                    {
                        EstimationId = (int)ScenarioContext.Current[EstimationId],
                        Quantity = new decimal(14.5)
                    }
                }
            };

            var buildingId = ScenarioContext.Current[BuildingId];
            var reportId = Client.Put($"buildings/{buildingId}/reports", report);
            ScenarioContext.Current[ReportId] = reportId;
            ScenarioContext.Current[ReportDate] = report.Date;
            ScenarioContext.Current[EstimationId] = report.Work.ToArray()[0].EstimationId;
        }
        
        [Then(@"I can get Report")]
        public void ThenICanGetReport()
        {
            var reportId = ScenarioContext.Current[ReportId];
            var buildingId = ScenarioContext.Current[BuildingId];
            var report = Client.Get<ReportDTO>($"buildings/{buildingId}/reports/{reportId}");
            
            var reportDate = ScenarioContext.Current[ReportDate];
            Assert.That(report.Date, Is.EqualTo(reportDate));
        }
        
        [Then(@"I can update Report")]
        public void ThenICanUpdateReport()
        {
            var report = new ReportDTO
            {
                Date = DateTime.UtcNow,
                NumberOfWorkers = 14,
                Work = new[]{
                    new ReportQuantity
                    {
                        EstimationId = (int)ScenarioContext.Current[EstimationId],
                        Quantity = new decimal(17)
                    }
                }
            };

            var reportId = ScenarioContext.Current[ReportId];
            var buildingId = ScenarioContext.Current[BuildingId];
            Client.Post($"buildings/{buildingId}/reports/{reportId}", report);

            ScenarioContext.Current[ReportDate] = report.Date;
        }
        
        [Then(@"Report is on list")]
        public void ThenReportIsOnList()
        {
            var reportDate = (DateTime)ScenarioContext.Current[ReportDate];
            var reportDateDto = new ReportDate
            {
                Month = reportDate.Month,
                Year = reportDate.Year
            };
            
            var buildingId = ScenarioContext.Current[BuildingId];
            var reports = Client.Post<IEnumerable<ReportList>, ReportDate>($"buildings/{buildingId}/reports", reportDateDto);

            var reportId = (int) ScenarioContext.Current[ReportId];
            Assert.IsTrue(reports.SelectMany(r => r.Reports).Any(r => r.Id == reportId && r.UserName.Contains("MacBub")));
        }
        
        [Then(@"Report is add to estimation")]
        public void ThenReportIsAddToEstimation()
        {
            var reportDate = (DateTime)ScenarioContext.Current[ReportDate];
            var reportDateDto = new ReportDate
            {
                Month = reportDate.Month,
                Year = reportDate.Year
            };

            var buildingId = ScenarioContext.Current[BuildingId];
            var estimationId = ScenarioContext.Current[EstimationId];

            var reports = 
                Client.Get<IEnumerable<EstimationReport>>($"buildings/{buildingId}/estimations/{estimationId}/reports");

            Assert.IsTrue(reports.Any(r => r.UserName.Contains("MacBub") && r.Date == reportDate));
        }
    }
    public class ReportList
    {
        public int Day { get; set; }
        public IEnumerable<ReportListItem> Reports { get; set; }
    }

    public class ReportListItem
    {
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public int Id { get; set; }
    }

    public class EstimationReport{
        public string CompanyName { get; set; } 
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public decimal Quantity { get; set; }
    }
    public class ReportDTO
    {
        public bool CanEdit { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfWorkers { get; set; }
        public IEnumerable<ReportQuantity> Work { get; set; }
    }

    public class ReportQuantity
    {
        public int EstimationId { get; set; }
        public decimal Quantity { get; set; }
    }

    public class ReportDate
    {
        public int Year { get; set; }
        public int Month { get; set; }
    }
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
        public int CompanyId { get; set; }
    }
}
