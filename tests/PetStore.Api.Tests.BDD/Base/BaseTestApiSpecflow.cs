using CleanTest.Framework.Drivers.ApiDriver.Adapters;
using CleanTest.Framework.Drivers.ApiDriver.Interfaces;
using CleanTest.Framework.Drivers.WebDriver.Enums;
using Reqnroll;

namespace PetStore.Api.Tests.BDD.Base;

public class BaseTestApiSpecflow
{
    protected static IApiDriverAdapter ApiDriver { get; private set; }
    protected const string BaseUrl = "https://petstore.swagger.io";
    
    protected readonly ScenarioContext _scenarioContext;
    protected readonly FeatureContext _featureContext;

    public BaseTestApiSpecflow(ScenarioContext scenarioContext, FeatureContext featureContext)
    {
        _scenarioContext = scenarioContext;
        _featureContext = featureContext;
    }

    public static void InitializeApiDriver(ApiDriverType apiDriverType)
    {
        if (ApiDriver != null) return; // Prevent multiple initializations

        ApiDriver = apiDriverType switch
        {
            ApiDriverType.Playwright => new PlaywrightApiAdapter(BaseUrl),
            ApiDriverType.RestSharp => new RestSharpAdapter(BaseUrl),
            _ => throw new ArgumentException($"Unsupported API client type: {apiDriverType}")
        };
        Console.WriteLine($"Using {apiDriverType} for API communication");
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
