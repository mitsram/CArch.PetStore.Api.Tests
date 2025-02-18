using PetStore.Api.Application.Repositories;
using PetStore.Api.Application.UseCases;
using Reqnroll;

namespace PetStore.Api.Tests.BDD.Base;

public abstract class BaseStepDefinitions : BaseTestApiSpecflow
{
    protected readonly PetUseCases _petUseCases;

    protected BaseStepDefinitions(ScenarioContext scenarioContext, FeatureContext featureContext) 
        : base(scenarioContext, featureContext)
    {
        var petRepository = new PetRepository(ApiDriver);
        _petUseCases = new PetUseCases(petRepository);
    }

    protected void SetCurrentPetId(long petId)
    {
        _scenarioContext["CurrentPetId"] = petId;
    }

    protected long GetCurrentPetId()
    {
        if (!_scenarioContext.ContainsKey("CurrentPetId"))
        {
            throw new InvalidOperationException("No pet ID found in context. Please ensure a pet was added first.");
        }
        return (long)_scenarioContext["CurrentPetId"];
    }

    protected void SetDeleteResult(bool result)
    {
        _scenarioContext["DeleteResult"] = result;
    }

    protected bool GetDeleteResult()
    {
        if (!_scenarioContext.ContainsKey("DeleteResult"))
        {
            throw new InvalidOperationException("No delete result found in context. Please ensure delete operation was performed.");
        }
        return _scenarioContext.Get<bool>("DeleteResult");
    }
}