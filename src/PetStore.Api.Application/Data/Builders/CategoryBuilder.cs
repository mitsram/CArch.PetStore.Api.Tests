using PetStore.Api.Domain.Entities;

namespace PetStore.Api.Application.Data.Builders;

public class CategoryBuilder
{
    private long _id;
    private string _name;

    private CategoryBuilder()
    {
    }

    public static CategoryBuilder Empty() => new();

    public CategoryBuilder Id(long id)
    {
        _id = id;
        return this;
    }

    public CategoryBuilder Name(string name)
    {
        _name = name;
        return this;
    }

    public Category Build()
    {
        return new Category()
        {
            Id = _id,
            Name = _name
        };
    }
}