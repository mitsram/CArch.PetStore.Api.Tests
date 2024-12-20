Feature: Pet API
As a pet store owner
I want to manage pets in my store
So that I can keep track of available pets

Scenario: Add a new pet
    Given I have a new pet with the following details:
    """
    {
      "name": "Fluffy",
      "status": "available",
      "photoUrls": ["http://example.com/fluffy.jpg"],
      "category": {
        "name": "Dogs"
      },
      "tags": [
        {
          "name": "Friendly"
        }
      ]
    }
    """
    When I add the pet to the store
    Then the pet should be successfully added
    And the response should contain:
    """
    {
      "id": "{id}",
      "name": "Fluffy",
      "status": "available",
      "photoUrls": ["http://example.com/fluffy.jpg"],
      "category": {
        "id": "{category_id}",
        "name": "Dogs"
      },
      "tags": [
        {
          "id": "{tag_id}",
          "name": "Friendly"
        }
      ]
    }
    """

Scenario: Retrieve a pet by ID
    Given I have added a pet with the following details:
    """
    {
      "name": "Buddy",
      "status": "available"
    }
    """
    When I retrieve the pet by its ID
    Then the response should contain:
    """
    {
      "id": "{id}",
      "name": "Buddy",
      "status": "available"
    }
    """

Scenario: Update an existing pet
    Given I have added a pet with the following details:
    """
    {
      "name": "Max",
      "status": "available"
    }
    """
    When I update the pet with the following details:
    """
    {
      "status": "sold"
    }
    """
    Then the response should contain:
    """
    {
      "id": "{id}",
      "name": "Max",
      "status": "sold"
    }
    """

Scenario: Delete a pet
    Given I have added a pet with the following details:
    """
    {
      "name": "Charlie",
      "status": "available"
    }
    """
    When I delete the pet
    Then the pet should be successfully deleted
    And the response should indicate successful deletion
