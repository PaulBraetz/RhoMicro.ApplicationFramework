using Microsoft.Extensions.Hosting;

using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Composition;
using RhoMicro.ApplicationFramework.Hosting;

using SimpleInjector;

using SplitAspectsLib;

using SplitAspectsTestApp;

var app = CliApp.CreateBuilder()
    .ConfigureOptions(o => o.AppRunOptions.Logger = NullContainerLogger.Instance)
    .ConfigureOptions(o => o.Composer += AspectComposers.CreateDefault(
        Lifestyle.Singleton,
        CommonAspects.None,
        interceptors => interceptors.Append<ToUpperInterceptor, String>()) + 
        Composer.Create(c =>
    {
        c.RegisterServices(options: new()
        {
            RegistrationPredicate = ConventionalServiceRegistrationPredicates.RegisterAll,
            RegistrationDerivation = info =>
            {
                var result = ConventionalServiceRegistrationDerivations.Default.Invoke(info)
                    .Select(
                        r => r.ImplementationType.Assembly == typeof(ConcatServiceImpl).Assembly
                        ? r with { OverridePreexistingRegistration = true, Lifestyle = Lifestyle.Singleton }
                        : r with { Lifestyle = Lifestyle.Singleton });

                return result;
            }
        }, typeof(IConcatService).Assembly, typeof(ConcatServiceImpl).Assembly);
    }))
    .AddHostedServices(typeof(MainService).Assembly)
    .AddConsoleLogging()
    .Build();

await app.RunAsync(default).ConfigureAwait(false);

class MainService(IConcatService concatService, IHostLifetime hostLifetime) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var msg = await concatService.Concat("Hello, ", "World!", cancellationToken);
        Console.WriteLine(msg);
        await hostLifetime.StopAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

class ToUpperInterceptor : IInterceptor<String>
{
    public ValueTask<String> Intercept(String obj, CancellationToken cancellationToken) => ValueTask.FromResult(obj.ToUpperInvariant());
}