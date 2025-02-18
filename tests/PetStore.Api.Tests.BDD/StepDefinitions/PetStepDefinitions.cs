
using PetStore.Api.Domain.Entities;
using PetStore.Api.Tests.BDD.Base;
using Reqnroll;



namespace PetApi.Tests.Specflow.StepDefinitions;

[Binding]
public class PetStepDefinitions : BaseStepDefinitions
{
    private Pet _testPet;
    private Pet _resultPet;

    public PetStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) 
        : base(scenarioContext, featureContext)
    {
    }

    [When(@"I add a new pet with the following details:")]
    public async Task WhenIAddANewPetWithTheFollowingDetails(DataTable table)
    {
        var petDetails = table.CreateInstance<PetDetails>();
        _testPet = new Pet
        {
            Name = petDetails.Name,
            Status = petDetails.Status,
            PhotoUrls = new List<string> { petDetails.PhotoUrl },
            Categories = new List<Category> { new Category { Id = 1, Name = petDetails.Category } },
            Tags = new List<Tag> { new Tag { Id = 1, Name = petDetails.Tag } }
        };

        _resultPet = await _petUseCases.AddPetAsync(_testPet);
        SetCurrentPetId(_resultPet.Id);
    }

    [Given(@"I have added a pet with name ""(.*)"" and status ""(.*)""")]
    public async Task GivenIHaveAddedAPetWithNameAndStatus(string name, string status)
    {
        _testPet = new Pet
        {
            Name = name,
            Status = status
        };
        _resultPet = await _petUseCases.AddPetAsync(_testPet);
        SetCurrentPetId(_resultPet.Id);
    }

    [When(@"I retrieve the pet by its id")]
    public async Task WhenIRetrieveThePetByItsId()
    {
        var petId = GetCurrentPetId();
        _resultPet = await _petUseCases.GetPetByIdAsync(petId);
    }

    [When(@"I update the pet with the following details:")]
    public async Task WhenIUpdateThePetWithTheFollowingDetails(DataTable table)
    {
        var petDetails = table.CreateInstance<PetDetails>();
        var petToUpdate = new Pet
        {
            Id = GetCurrentPetId(),
            Name = petDetails.Name,
            Status = petDetails.Status
        };

        _resultPet = await _petUseCases.UpdatePetAsync(petToUpdate);
    }

    [When(@"I delete the pet")]
    public async Task WhenIDeleteThePet()
    {
        var petId = GetCurrentPetId();
        var result = await _petUseCases.DeletePetAsync(petId);
        SetDeleteResult(result);
    }

    [Then(@"the pet should be created successfully")]
    public void ThenThePetShouldBeCreatedSuccessfully()
    {
        Assert.That(_resultPet, Is.Not.Null);
        Assert.That(_resultPet.Id, Is.GreaterThan(0));
    }

    [Then(@"the pet details should match the input")]
    public void ThenThePetDetailsShouldMatchTheInput()
    {
        Assert.That(_resultPet.Name, Is.EqualTo(_testPet.Name));
        Assert.That(_resultPet.Status, Is.EqualTo(_testPet.Status));
    }

    [Then(@"the pet details should be returned successfully")]
    public void ThenThePetDetailsShouldBeReturnedSuccessfully()
    {
        Assert.That(_resultPet, Is.Not.Null);
        Assert.That(_resultPet.Id, Is.EqualTo(GetCurrentPetId()));
    }

    [Then(@"the pet name should be ""(.*)""")]
    public void ThenThePetNameShouldBe(string expectedName)
    {
        Assert.That(_resultPet.Name, Is.EqualTo(expectedName));
    }

    [Then(@"the pet status should be ""(.*)""")]
    public void ThenThePetStatusShouldBe(string expectedStatus)
    {
        Assert.That(_resultPet.Status, Is.EqualTo(expectedStatus));
    }

    [Then(@"the pet should be updated successfully")]
    public void ThenThePetShouldBeUpdatedSuccessfully()
    {
        Assert.That(_resultPet, Is.Not.Null);
    }

    [Then(@"the pet should be successfully deleted")]
    public void ThenThePetShouldBeSuccessfullyDeleted()
    {
        Assert.That(GetDeleteResult(), Is.True);
    }
}

public class PetDetails
{
    public string Name { get; set; }
    public string Status { get; set; }
    public string PhotoUrl { get; set; }
    public string Category { get; set; }
    public string Tag { get; set; }
}

