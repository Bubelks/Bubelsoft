Feature: LogInTests

Scenario: Log in to api
	Given I am logged in as MacBub
	When I try to get current user
	Then Result should be OK
	And Data current user should be MacBub

Scenario: Get unauthorized
	Given I am not logged in
	When I try to get current user
	Then Result should be Unauthorized
