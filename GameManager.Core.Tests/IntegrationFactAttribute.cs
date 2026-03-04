namespace GameManager.Core.Tests;

public sealed class IntegrationFactAttribute : FactAttribute
{
    public IntegrationFactAttribute()
    {
        if ( Environment.GetEnvironmentVariable("RUN_INTEGRATION_TESTS") != "true" )
            Skip = "Integration test. Set RUN_INTEGRATION_TESTS=true to run.";
    }
}
