using PetStore.Api.Application.Repositories;
using PetStore.Api.Application.UseCases;
using PetStore.Api.Domain.Entities;

namespace PetApi.Tests.Specs;

[TestFixture]
public class PetApiTests : BaseTestApi
{
    private PetUseCases _petUseCases;
    private IPetRepository _petRepository;

    [SetUp]
    public void Setup()
    {
        base.SetUp();
        _petRepository = new PetRepository(apiDriver);
        _petUseCases = new PetUseCases(_petRepository);
    }

    [Test]
    public async Task AddPet_ShouldReturnCreatedPet()
    {
        // Arrange
        var newPet = new Pet
        {
            Name = "Fluffy",
            Status = "available",
            PhotoUrls = new List<string> { "http://example.com/fluffy.jpg" },
            Categories = new List<Category> { new Category { Id = 1, Name = "Dogs" } },
            Tags = new List<Tag> { new Tag { Id = 1, Name = "Friendly" } }
        };

        // Act
        var addedPet = await _petUseCases.AddPetAsync(newPet);

        // Assert
        Assert.That(addedPet, Is.Not.Null);
        Assert.That(addedPet.Id, Is.GreaterThan(0));
        Assert.That(addedPet.Name, Is.EqualTo(newPet.Name));
        Assert.That(addedPet.Status, Is.EqualTo(newPet.Status));
    }

    [Test]
    public async Task GetPet_ShouldReturnExistingPet()
    {
        // Arrange
        var newPet = new Pet
        {
            Name = "Buddy",
            Status = "available",
            PhotoUrls = new List<string> { "http://example.com/buddy.jpg" },
            Categories = new List<Category> { new Category { Id = 1, Name = "Dogs" } },
            Tags = new List<Tag> { new Tag { Id = 1, Name = "Playful" } }
        };
        var addedPet = await _petUseCases.AddPetAsync(newPet);

        // Act
        var retrievedPet = await _petUseCases.GetPetByIdAsync(addedPet.Id);

        // Assert
        Assert.That(retrievedPet, Is.Not.Null);
        Assert.That(retrievedPet.Id, Is.EqualTo(addedPet.Id));
        Assert.That(retrievedPet.Name, Is.EqualTo(addedPet.Name));
        Assert.That(retrievedPet.Status, Is.EqualTo(addedPet.Status));
    }

    [Test]
    public async Task UpdatePet_ShouldReturnUpdatedPet()
    {
        // Arrange
        var newPet = new Pet
        {
            Name = "OldDog",
            Status = "available",
            Category = new Category { Id = 1, Name = "Dogs" }
        };
        var addedPet = await _petUseCases.AddPetAsync(newPet);

        var petToUpdate = new Pet
        {
            Id = addedPet.Id,
            Name = "UpdatedDog",
            Status = "sold",
            Category = new Category { Id = 1, Name = "Dogs" }
        };

        // Act
        var updatedPet = await _petUseCases.UpdatePetAsync(petToUpdate);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(updatedPet, Is.Not.Null);
            Assert.That(updatedPet.Name, Is.EqualTo(petToUpdate.Name));
            Assert.That(updatedPet.Status, Is.EqualTo(petToUpdate.Status));
        });
    }

    [Test]
    public async Task DeletePet_ShouldReturnSuccess()
    {
        // Arrange
        var newPet = new Pet
        {
            Name = "ToBeDeleted",
            Status = "available"
        };
        var addedPet = await _petUseCases.AddPetAsync(newPet);

        // Act
        var result = await _petUseCases.DeletePetAsync(addedPet.Id);

        // Assert
        Assert.That(result, Is.True);
    }
}
