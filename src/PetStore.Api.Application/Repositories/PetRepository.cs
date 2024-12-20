using System.Text.Json;
using CleanTest.Framework.Drivers.ApiDriver.Interfaces;
using PetStore.Api.Domain.Entities;

namespace PetStore.Api.Application.Repositories;

public class PetRepository : IPetRepository
{
    private readonly IApiDriverAdapter _apiDriver;

    public PetRepository(IApiDriverAdapter apiDriver)
    {
        _apiDriver = apiDriver;
    }

    public async Task<Pet> AddPetAsync(Pet pet)
    {
        var response = await _apiDriver.SendRequestAsync("POST", "pet", pet);
        Console.WriteLine($"Response Status: {response.StatusCode}");
        Console.WriteLine($"Response Content: {response.Content}");

        if (response.StatusCode == 200 && !string.IsNullOrEmpty(response.Content))
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var deserializedPet = JsonSerializer.Deserialize<Pet>(response.Content, options);
                Console.WriteLine($"Deserialized Pet: {JsonSerializer.Serialize(deserializedPet)}");
                return deserializedPet;
            }
            catch (JsonException ex)
            {
                throw new Exception($"Failed to deserialize the response content: {ex.Message}");
            }
        }
        else
        {
            throw new Exception($"API request failed: Status Code {response.StatusCode}");
        }
    }

    public async Task<Pet> GetPetByIdAsync(long petId)
    {
        var response = await _apiDriver.SendRequestAsync("GET", $"pet/{petId}");
        Console.WriteLine($"GetPetByIdAsync Response Status: {response.StatusCode}");
        Console.WriteLine($"GetPetByIdAsync Response Content: {response.Content}");

        if (response.StatusCode == 200 && !string.IsNullOrEmpty(response.Content))
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var deserializedPet = JsonSerializer.Deserialize<Pet>(response.Content, options);
                Console.WriteLine($"GetPetByIdAsync Deserialized Pet: {JsonSerializer.Serialize(deserializedPet)}");
                return deserializedPet;
            }
            catch (JsonException ex)
            {
                throw new Exception($"Failed to deserialize the response content in GetPetByIdAsync: {ex.Message}");
            }
        }
        else
        {
            throw new Exception($"API request failed in GetPetByIdAsync: Status Code {response.StatusCode}");
        }
    }

    public async Task<Pet> UpdatePetAsync(Pet pet)
    {
        var response = await _apiDriver.SendRequestAsync("PUT", "pet", pet);
        Console.WriteLine($"UpdatePetAsync Response Status: {response.StatusCode}");
        Console.WriteLine($"UpdatePetAsync Response Content: {response.Content}");

        if (response.StatusCode == 200 && !string.IsNullOrEmpty(response.Content))
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var deserializedPet = JsonSerializer.Deserialize<Pet>(response.Content, options);
                Console.WriteLine($"UpdatePetAsync Deserialized Pet: {JsonSerializer.Serialize(deserializedPet)}");
                return deserializedPet;
            }
            catch (JsonException ex)
            {
                throw new Exception($"Failed to deserialize the response content in UpdatePetAsync: {ex.Message}");
            }
        }
        else
        {
            throw new Exception($"API request failed in UpdatePetAsync: Status Code {response.StatusCode}");
        }
    }

    public async Task<bool> DeletePetAsync(long petId)
    {
        var response = await _apiDriver.SendRequestAsync("DELETE", $"pet/{petId}");
        return response.StatusCode == 200;
    }
}
