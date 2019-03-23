Feature: ReportTest

Scenario: Deal with report
	Given I logged as MacBub
	And I have access to Building
	And Building have estimation
	When I create Report
	Then I can get Report
	And I can update Report
	And Report is on list
	And Report is add to estimation
