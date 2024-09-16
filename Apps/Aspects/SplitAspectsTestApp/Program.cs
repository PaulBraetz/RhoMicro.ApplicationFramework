
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using RhoMicro.ApplicationFramework.Composition;
using RhoMicro.ApplicationFramework.Hosting;

using SplitAspectsLib;

await CliApp.CreateBuilder()
    .ConfigureOptions(o => o.Composer += Composer.Create(c =>
    {
        c.RegisterServices(typeof(IConcatService).Assembly);
    }))
    .ConfigureCapabilities(c=>c.Services.AddHostedService<MainService>())
    .Build()
    .RunAsync(default)
    .ConfigureAwait(false);

internal class MainService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Hello, World!");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}