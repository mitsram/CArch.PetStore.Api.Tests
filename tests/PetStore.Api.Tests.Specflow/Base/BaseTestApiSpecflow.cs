using CleanTest.Framework.Drivers.ApiDriver.Adapters;
using CleanTest.Framework.Drivers.ApiDriver.Interfaces;
using TechTalk.SpecFlow;

namespace PetStore.Api.Tests.Specflow.Base;

public class BaseTestApiSpecflow
{
    protected static IApiDriverAdapter ApiDriver { get; private set; }
    protected const string BaseUrl = "https://petstore.swagger.io/v2";
    
    protected readonly ScenarioContext _scenarioContext;
    protected readonly FeatureContext _featureContext;

    public BaseTestApiSpecflow(ScenarioContext scenarioContext, FeatureContext featureContext)
    {
        _scenarioContext = scenarioContext;
        _featureContext = featureContext;
    }

    public static void InitializeApiDriver()
    {
        if (ApiDriver != null) return; // Prevent multiple initializations

        string apiClientType = Environment.GetEnvironmentVariable("API_CLIENT_TYPE") ?? "RestSharp";

        ApiDriver = apiClientType.ToLower() switch
        {
            "httpclient" => new HttpClientAdapter(BaseUrl),
            "restsharp" => new RestSharpAdapter(BaseUrl),
            _ => throw new ArgumentException($"Unsupported API client type: {apiClientType}")
        };

        Console.WriteLine($"Using {apiClientType} for API communication");
    }

    public static void CleanupApiDriver()
    {
        if (ApiDriver != null)
        {
            ApiDriver.Dispose();
            ApiDriver = null;
        }
    }
}
