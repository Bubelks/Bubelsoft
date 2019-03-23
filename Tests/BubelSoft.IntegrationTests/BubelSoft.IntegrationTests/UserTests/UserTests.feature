Feature: UserTests

Scenario: NewUser
	Given I logged as MacBub
	When I add new worker to company
	And I registry new user
	And I login as new user
	Then I have user on workers list
