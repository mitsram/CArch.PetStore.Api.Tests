Feature: Pet Store API
As a pet store owner
I want to manage pets in the store
So that I can keep track of available pets

Scenario: Add a new pet to the store
    When I add a new pet with the following details:
        | Name    | Status    | PhotoUrl                        | Category | Tag      |
        | Fluffy  | available | http://example.com/fluffy.jpg   | Dogs     | Friendly |
    Then the pet should be created successfully
    And the pet details should match the input

Scenario: Retrieve an existing pet
    Given I have added a pet with name "Buddy" and status "available"
    When I retrieve the pet by its id
    Then the pet details should be returned successfully
    And the pet name should be "Buddy"
    And the pet status should be "available"

Scenario: Update an existing pet
    Given I have added a pet with name "OldDog" and status "available"
    When I update the pet with the following details:
        | Name       | Status |
        | UpdatedDog | sold   |
    Then the pet should be updated successfully
    And the pet name should be "UpdatedDog"
    And the pet status should be "sold"

Scenario: Delete an existing pet
    Given I have added a pet with name "ToBeDeleted" and status "available"
    When I delete the pet    
    Then the pet should be successfully deleted
