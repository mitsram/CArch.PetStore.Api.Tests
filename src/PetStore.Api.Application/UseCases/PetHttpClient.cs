using System.Text.Json;
using System.Text;
using PetStore.Api.Domain.Entities;

namespace PetStore.Api.Application.UseCases;

public class PetHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl;

    public PetHttpClient(string apiUrl = "https://alert-api.example.com/cwms")
    {
        _httpClient = new HttpClient();
        _apiUrl = apiUrl;
    }

    public async Task<HttpResponseMessage> SendToCwms(Pet jsonMessage)
    {
        try
        {
            var json = JsonSerializer.Serialize(jsonMessage);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync(_apiUrl, content);
            response.EnsureSuccessStatusCode();
            return response;
        }
        catch (HttpRequestException ex)
        {
            // Log error or handle it as needed
            throw new Exception($"Failed to send alert to CWMS: {ex.Message}", ex);
        }
    }
    
} 