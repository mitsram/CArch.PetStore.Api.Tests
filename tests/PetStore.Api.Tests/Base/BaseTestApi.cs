using CleanTest.Framework.Drivers.ApiDriver.Interfaces;
using CleanTest.Framework.Factories;

namespace PetApi.Tests
{
    public class BaseTestApi
    {
        protected IApiDriverAdapter apiDriver;
        protected const string BaseUrl = "https://petstore.swagger.io/v2";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            // Global setup code, if any
            Console.WriteLine("Starting test suite execution...");
        }

        [SetUp]
        public void SetUp()
        {
            apiDriver = ApiDriverFactory.Create(BaseUrl, "RestSharp");
        }

        [TearDown]
        public void TearDown()
        {
            // Common teardown code, if any
            Console.WriteLine("Test completed.");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Global teardown code, if any
            Console.WriteLine("Test suite execution completed.");
        }

        protected void LogTestInfo(string message)
        {
            Console.WriteLine($"[{TestContext.CurrentContext.Test.Name}] {message}");
        }
    }
}
