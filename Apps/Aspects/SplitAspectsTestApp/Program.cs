using Microsoft.Extensions.Hosting;

using RhoMicro.ApplicationFramework.Composition;
using RhoMicro.ApplicationFramework.Hosting;

using SimpleInjector;

using SplitAspectsLib;

using SplitAspectsTestApp;

var app = CliApp.CreateBuilder()
    .ConfigureOptions(o => o.Composer += AspectComposers.Create(Lifestyle.Singleton, CommonAspects.All) + Composer.Create(c =>
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
    .AddTimeout(Lifestyle.Singleton)
    .Build();

await app.RunAsync(default).ConfigureAwait(false);

internal class MainService(IConcatService concatService) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        var msg = concatService.Concat("Hello, ", "World!", cancellationToken);
        Console.WriteLine(msg);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}