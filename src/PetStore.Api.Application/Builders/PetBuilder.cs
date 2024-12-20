using System.Text.Json;
using PetStore.Api.Domain.Entities;

namespace PetStore.Api.Application.Builders;

public class PetBuilder
{
    private long _id;
    private string _name;
    private string _status;
    private readonly CategoryBuilder _categoryBuilder = CategoryBuilder.Empty();
    private readonly List<CategoryBuilder> _categoriesBuilder = new();    
    private readonly List<TagBuilder> _tagsBuilder = new();
    private readonly List<string> _photoUrls = new();
    private const string ResourceNamespace = "PetApi.Application.Builders.Json.";

    private PetBuilder()
    {
    }

    public static PetBuilder Empty() => new();
    public static PetBuilder Create()
    {
        return CreateFromJson(ResourceNamespace + "Pet.json");
    }

    private static PetBuilder CreateFromJson(string resourcePath)
    {
        var builder = new PetBuilder();
        var pet = LoadFromJson(resourcePath);
        
        return builder
            .WithId(pet.Id)
            .WithName(pet.Name)
            .WithCategory(c => c
                .Id(pet.Category.Id)
                .Name(pet.Category.Name)
            );
    }


    public PetBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public PetBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public PetBuilder WithStatus(string status)
    {
        _status = status;
        return this;
    }

    public PetBuilder WithCategory(Action<CategoryBuilder> action)
    {
        action(_categoryBuilder);
        return this;
    }

    public PetBuilder WithCategories(Action<List<CategoryBuilder>> action)
    {
        action(_categoriesBuilder);
        return this;
    }

    public PetBuilder WithTags(Action<List<TagBuilder>> action)
    {
        action(_tagsBuilder);
        return this;
    }

    public PetBuilder WithPhotoUrls(List<string> urls)
    {
        _photoUrls.AddRange(urls);
        return this;
    }

    public PetBuilder WithPhotoUrls(params string[] urls)
    {
        _photoUrls.AddRange(urls);
        return this;
    }

    public PetBuilder WithPhotoUrl(string url)
    {
        _photoUrls.Add(url);
        return this;
    }

    public Pet Build()
    {
        return new Pet()
        {
            Id = _id,
            Name = _name,
            Status = _status,
            Category = _categoryBuilder.Build(),
            Categories = _categoriesBuilder.Select(c => c.Build()).ToList(),
            Tags = _tagsBuilder.Select(t => t.Build()).ToList(),
            PhotoUrls = _photoUrls,
        };
    }

    private static Pet LoadFromJson(string resourcePath)
    {
        using var stream = typeof(PetBuilder).Assembly
            .GetManifestResourceStream(resourcePath);
        
        if (stream == null)
        {
            throw new InvalidOperationException(
                $"Order JSON resource not found: {resourcePath}");
        }

        using var reader = new StreamReader(stream);
        var jsonContent = reader.ReadToEnd();
        
        return JsonSerializer.Deserialize<Pet>(jsonContent) 
            ?? throw new InvalidOperationException("Failed to deserialize order");
    }
}