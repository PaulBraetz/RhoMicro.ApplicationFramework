namespace RhoMicro.ApplicationFramework.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using SimpleInjector;

/// <summary>
/// Contains tests for api service endpoint setups.
/// </summary>
public class ApiServiceEndpointsTests
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [Fact]
    public async Task MapApiServiceEndpoints_throws_validation_exception_for_nonexistent_request_type()
    {
        var app = WebServerGuiApp.CreateBuilder()
            .ConfigureCapabilities(c =>
                c.Configuration.AddInMemoryCollection(new Dictionary<String, String?>()
                {
                    ["ApiServicesSettings:BaseUri"] = "https://localhost:7072",
                    ["ApiServicesSettings:Services:0:Endpoint"] = "/authentication/does-citizen-exist",
                    ["ApiServicesSettings:Services:0:Request"] = "faketype"
                }))
            .AddApiServiceEndpoints()
            .Build()
            .MapApiServiceEndpoints();

        var cts = new CancellationTokenSource(100);
        var ioeEx1 = await Assert.ThrowsAsync<InvalidOperationException>(async () => await app.RunAsync(cts.Token));
        var activationEx = Assert.IsType<ActivationException>(ioeEx1.InnerException);
        var ioeEx2 = Assert.IsType<InvalidOperationException>(activationEx.InnerException);
        var ovEx = Assert.IsType<OptionsValidationException>(ioeEx2.InnerException);
        Assert.Equal("Endpoints with invalid request type or request uri detected. Service endpoint uris must be well-formed relative uris.", ovEx.Message);
    }
}