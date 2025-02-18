
using Reqnroll;

namespace PetStore.Api.Tests.BDD.Base;

[Binding]
public class Hook : BaseTestApiSpecflow
{
    public Hook(ScenarioContext scenarioContext, FeatureContext featureContext) 
        : base(scenarioContext, featureContext)
    {
    }

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        Console.WriteLine("Starting test suite execution...");
    }

    [AfterTestRun]
    public static void AfterTestRun()
    {
        CleanupApiDriver();
        Console.WriteLine("Test suite execution completed.");
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        if (_scenarioContext.ScenarioInfo.Tags.Contains("skip-in-prod") && 
            Environment.GetEnvironmentVariable("ENV") == "PROD")
        {
            _scenarioContext.Pending();
            return;
        }

        InitializeApiDriver();
        Console.WriteLine($"Starting scenario: {_scenarioContext.ScenarioInfo.Title}");
    }

    [AfterScenario]
    public void AfterScenario()
    {
        try 
        {
            if (_scenarioContext.TestError != null)
            {
                Console.WriteLine($"Scenario failed: {_scenarioContext.TestError.Message}");
                Console.WriteLine($"Stack trace: {_scenarioContext.TestError.StackTrace}");
            }
        }
        finally
        {
            Console.WriteLine($"Scenario completed: {_scenarioContext.ScenarioInfo.Title}");
        }
    }
}
