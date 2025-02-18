using System.Text.Json;
using System.Text.Json.Serialization;
using PetStore.Api.Application.Repositories;
using PetStore.Api.Application.UseCases;
using PetStore.Api.Domain.Entities;
using PetStore.Api.Tests.BDD.Base;
using Reqnroll;

namespace PetApi.Tests.Specflow.StepDefinitions;

[Binding]
public class PetWithJsonStepDefinitions : BaseStepDefinitions
{
    private readonly PetUseCases _petUseCases;
    private Pet _testPet;

    public PetWithJsonStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) 
        : base(scenarioContext, featureContext)
    {
        var petRepository = new PetRepository(ApiDriver);
        _petUseCases = new PetUseCases(petRepository);
    }

    [Given(@"I have a new pet with the following details:")]
    [Given(@"I have added a pet with the following details:")]
    public async Task GivenIHaveANewPetWithTheFollowingDetails(string petJson)
    {
        try
        {
            // Deserialize the pet JSON
            _testPet = JsonSerializer.Deserialize<Pet>(petJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Immediately add the pet to the store
            var addedPet = await _petUseCases.AddPetAsync(_testPet);
            _scenarioContext["AddedPet"] = addedPet;
            SetCurrentPetId(addedPet.Id);

            Console.WriteLine($"Added pet with ID: {addedPet.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding pet: {ex.Message}");
            throw;
        }
    }

    [When(@"I add the pet to the store")]
    public async Task WhenIAddThePetToTheStore()
    {
        var addedPet = await _petUseCases.AddPetAsync(_testPet);
        _scenarioContext["AddedPet"] = addedPet;
    }

    [Then(@"the pet should be successfully added")]
    public void ThenThePetShouldBeSuccessfullyAdded()
    {
        var addedPet = _scenarioContext.Get<Pet>("AddedPet");
        Assert.That(addedPet, Is.Not.Null);
        Assert.That(addedPet.Id, Is.GreaterThan(0));
    }

    [When(@"I retrieve the pet by its ID")]
    public async Task WhenIRetrieveThePetByItsID()
    {
        var addedPet = _scenarioContext.Get<Pet>("AddedPet");
        var retrievedPet = await _petUseCases.GetPetByIdAsync(addedPet.Id);
        _scenarioContext["RetrievedPet"] = retrievedPet;
    }

    [When(@"I update the pet with the following details:")]
    public async Task WhenIUpdateThePetWithTheFollowingDetails(string updateJson)
    {
        try
        {
            // Get the current pet ID
            var currentPetId = GetCurrentPetId();
            
            // Get the existing pet
            var existingPet = await _petUseCases.GetPetByIdAsync(currentPetId);
            if (existingPet == null)
            {
                throw new InvalidOperationException($"Pet with ID {currentPetId} not found");
            }

            // Create options for JSON deserialization
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            // Parse the update JSON as a dictionary to handle partial updates
            var updateData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(updateJson, options);
            
            // Create pet object for update, starting with existing values
            var petToUpdate = new Pet
            {
                Id = existingPet.Id,
                Name = existingPet.Name,
                Status = existingPet.Status,
                PhotoUrls = existingPet.PhotoUrls,
                Categories = existingPet.Categories,
                Tags = existingPet.Tags
            };

            // Apply only the properties that are present in the update JSON
            foreach (var kvp in updateData)
            {
                switch (kvp.Key.ToLowerInvariant())
                {
                    case "name":
                        petToUpdate.Name = kvp.Value.GetString();
                        break;
                    case "status":
                        petToUpdate.Status = kvp.Value.GetString();
                        break;
                    case "photourls":
                        petToUpdate.PhotoUrls = JsonSerializer.Deserialize<List<string>>(kvp.Value.GetRawText(), options);
                        break;
                    case "category":
                        petToUpdate.Categories = new List<Category> 
                        { 
                            JsonSerializer.Deserialize<Category>(kvp.Value.GetRawText(), options) 
                        };
                        break;
                    case "tags":
                        petToUpdate.Tags = JsonSerializer.Deserialize<List<Tag>>(kvp.Value.GetRawText(), options);
                        break;
                }
            }

            var updatedPet = await _petUseCases.UpdatePetAsync(petToUpdate);
            _scenarioContext["UpdatedPet"] = updatedPet;
            Console.WriteLine($"Updated pet with ID: {updatedPet.Id}, Name: {updatedPet.Name}, Status: {updatedPet.Status}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating pet: {ex.Message}");
            throw;
        }
    }

    [Then(@"the response should contain:")]
    public void ThenTheResponseShouldContain(string expectedJson)
    {
        // Get the most recently modified pet
        var actualPet = _scenarioContext.ContainsKey("UpdatedPet") 
            ? _scenarioContext.Get<Pet>("UpdatedPet")
            : _scenarioContext.ContainsKey("RetrievedPet") 
                ? _scenarioContext.Get<Pet>("RetrievedPet")
                : _scenarioContext.Get<Pet>("AddedPet");

        // Deserialize both JSONs to compare their content rather than their string representation
        var options = new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        // Replace placeholders in expected JSON
        expectedJson = expectedJson.Replace("{id}", actualPet.Id.ToString());
        if (actualPet.Category != null)
            expectedJson = expectedJson.Replace("{category_id}", actualPet.Category.Id.ToString());
        if (actualPet.Tags != null && actualPet.Tags.Count > 0)
            expectedJson = expectedJson.Replace("{tag_id}", actualPet.Tags[0].Id.ToString());

        // Parse both JSONs to dictionaries with case-insensitive keys
        var expected = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(expectedJson, options)
            .ToDictionary(kvp => kvp.Key.ToLowerInvariant(), kvp => kvp.Value);
        
        var actual = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(
            JsonSerializer.Serialize(actualPet, options), 
            options
        ).ToDictionary(kvp => kvp.Key.ToLowerInvariant(), kvp => kvp.Value);

        // Compare each property
        foreach (var kvp in expected)
        {
            var key = kvp.Key.ToLowerInvariant();
            Assert.That(actual.ContainsKey(key), 
                $"Expected property '{kvp.Key}' not found in actual response");

            var expectedValue = NormalizeJsonValue(kvp.Value);
            var actualValue = NormalizeJsonValue(actual[key]);

            Assert.That(actualValue, Is.EqualTo(expectedValue), 
                $"Property '{kvp.Key}' value mismatch.\nExpected: {expectedValue}\nActual: {actualValue}");
        }
    }

    private string NormalizeJsonValue(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Number:
                return element.GetRawText().ToLowerInvariant();
            case JsonValueKind.String:
                return element.GetString()?.ToLowerInvariant() ?? "";
            case JsonValueKind.True:
                return "true";
            case JsonValueKind.False:
                return "false";
            case JsonValueKind.Null:
                return "null";
            case JsonValueKind.Array:
                var array = element.EnumerateArray()
                    .Select(e => NormalizeJsonValue(e))
                    .OrderBy(s => s);
                return $"[{string.Join(",", array)}]";
            case JsonValueKind.Object:
                var obj = element.EnumerateObject()
                    .OrderBy(p => p.Name)
                    .Select(p => $"{p.Name.ToLowerInvariant()}:{NormalizeJsonValue(p.Value)}");
                return $"{{{string.Join(",", obj)}}}";
            default:
                return element.GetRawText().ToLowerInvariant();
        }
    }

    private string NormalizeJson(string json)
    {
        try
        {
            using var document = JsonDocument.Parse(json);
            return NormalizeJsonValue(document.RootElement);
        }
        catch (JsonException ex)
        {
            throw new ArgumentException($"Invalid JSON: {ex.Message}", nameof(json), ex);
        }
    }

    [Then(@"the response should indicate successful deletion")]
    public void ThenTheResponseShouldIndicateSuccessfulDeletion()
    {
        Assert.That(GetDeleteResult(), Is.True);
    }
}

