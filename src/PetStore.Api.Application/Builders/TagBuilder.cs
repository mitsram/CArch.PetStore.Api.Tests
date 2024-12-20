using PetStore.Api.Domain.Entities;

namespace PetStore.Api.Application.Builders;

public class TagBuilder
{
    private long _id;
    private string _name;

    private TagBuilder()
    {
    }

    public static TagBuilder Empty() => new();

    public TagBuilder Id(long id)
    {
        _id = id;
        return this;
    }

    public TagBuilder Name(string name)
    {
        _name = name;
        return this;
    }

    public Tag Build()
    {
        return new Tag()
        {
            Id = _id,
            Name = _name
        };
    }
}