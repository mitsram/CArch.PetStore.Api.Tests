using CleanTest.Framework.Drivers.ApiDriver.Builders;
// using JsonApi.Application.Services;
using PetStore.Api.Application.Data.Builders;
using PetStore.Api.Application.Repositories;
using PetStore.Api.Application.UseCases;
using PetStore.Api.Domain.Entities;


namespace PetApi.Tests.Specs;

[TestFixture]
public class PetApiWithBuilderTests : BaseTestApi
{
    private PetUseCases _petUseCases;    

    [SetUp]
    public void Setup()
    {
        base.SetUp();        
        _petUseCases = new PetUseCases(new PetRepository(apiDriver));
    }

    [Test]
    public async Task GetPet_ShouldReturnExistingPet()
    {
        // Arrange
        var newPet = PetBuilder.CreateNew()
            .WithName("Buddy")
            .WithStatus("available")
            .WithPhotoUrls(
                "http://example.com/buddy.jpg")
            .WithCategory(c => c
                .Id(1)
                .Name("Dogs"))
            .WithTags(t => {
                t.Add(TagBuilder.Empty()
                    .Id(1)
                    .Name("Playful"));
                t.Add(TagBuilder.Empty()
                    .Id(2)
                    .Name("Friendly"));
            })
            .Build();

        var addedPet = await _petUseCases.AddPetAsync(newPet);

        // Act
        var retrievedPet = await _petUseCases.GetPetByIdAsync(addedPet.Id);

        // Assert        
        Assert.Multiple(() =>
        {
            Assert.That(retrievedPet, Is.Not.Null);
            Assert.That(retrievedPet.Id, Is.EqualTo(addedPet.Id));
            Assert.That(retrievedPet.Name, Is.EqualTo(addedPet.Name));
            Assert.That(retrievedPet.Status, Is.EqualTo(addedPet.Status));
        });
    }
    
    
    public async Task<HttpResponseMessage> PostToApi(Pet order)
    {
        const string endpoint = "https://api.example.com/orders"; // Or get from configuration
        
        return await ApiBuilder
            .Endpoint(endpoint)
            .WithHeaders(headers =>
            {
                headers.Add("Content-Type", "application/json");
                headers.Add("Accept", "application/json");
            })
            .WithBody(order)
            .ExecutePost();
    }
}
