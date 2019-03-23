Feature: BuildingTests

Scenario: Adding company to building
	Given I logged as MacBub
	And I have access to Building
	When I add new Company to Building
	Then I have new Company on List

Scenario: Adding worker to building
	Given I logged as MacBub
	And I have access to Building
	When I add new User to my Company to Building
	Then I get new Worker on List
