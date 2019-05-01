Feature: CompanyTests

Scenario: Register new company
	When I register new company
	Then I can log in as company admin
	Then I can get new company info

Scenario: Add and Delete Worker
	Given I am logged in as MacBub
	When I add new worker to company
	Then I have user on workers list
	When I delete new Worker
	Then I do not get Worker on List

Scenario: Change company info
	Given I am logged in as MacBub
	And I get company info
	When I change company info
	Then I ensure that company info is changed
	And I rest company info
	