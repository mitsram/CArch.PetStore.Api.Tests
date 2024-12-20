namespace PetStore.Api.Domain.Entities;

public class Pet
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Status { get; set; }
    public Category? Category { get; set; }
    public List<string>? PhotoUrls { get; set; }
    public List<Category>? Categories { get; set; }
    public List<Tag>? Tags { get; set; }
}
