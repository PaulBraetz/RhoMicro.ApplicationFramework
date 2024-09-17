
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using RhoMicro.ApplicationFramework.Composition;
using RhoMicro.ApplicationFramework.Hosting;

using SplitAspectsLib;

using SplitAspectsTestApp;

await CliApp.CreateBuilder()
    .ConfigureOptions(o => o.Composer += Composer.Create(c =>
    {
        c.RegisterServices(typeof(IConcatService).Assembly);
        c.RegisterServices(typeof(ConcatServiceImpl).Assembly);
    }))
    .ConfigureCapabilities(c => c.Services.AddHostedService<MainService>())
    .Build()
    .RunAsync(default)
    .ConfigureAwait(false);

internal class MainService(IConcatService service) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var msg = service.Concat("Hello, ", "World!", default);
        Console.WriteLine(msg);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}