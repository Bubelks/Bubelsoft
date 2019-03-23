Feature: CompanyTests

Scenario: Add and Delete Worker
	Given I logged as MacBub
	When I add new worker to company
	Then I have user on workers list
	When I delete new Worker
	Then I do not get Worker on List

Scenario: Change company info
	Given I logged as MacBub
	And I get company info
	When I change company info
	Then I ensure that company info is changed
	And I rest company info
	