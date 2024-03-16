namespace RhoMicro.ApplicationFramework.Composition.Presentation.Views.Blazor;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// Contains composers able to compose blazor view object graphs.
/// </summary>
public static class Composers
{
    /// <summary>
    /// Gets the styles composer instance.
    /// </summary>
    public static IComposer CreateStylesComposer(IServiceCollection services, IConfiguration configuration) =>
        Composer.Create(c =>
        {
            var cssSettingsSection = configuration.GetSection("CssStyleSettings");

            var settingsTypes = typeof(CssStyleSettings).Assembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Settings", StringComparison.Ordinal) && t.IsAssignableTo(typeof(ICssStyle)));

            foreach(var settingsType in settingsTypes)
            {
                var optionsImplType = typeof(ConfigureNamedOptions<>).MakeGenericType(settingsType);
                var optionsServiceType = typeof(IOptions<>).MakeGenericType(settingsType);

                var cssStyleName = settingsType.Name[..^( "Settings".Length + 1 )];

                var instance = optionsImplType
                    .GetConstructor([typeof(String), typeof(Action<>).MakeGenericType(settingsType)])
                    ?.Invoke(null, [null, (Object settings) => cssSettingsSection.GetSection(cssStyleName).Bind(settings)])
                    ?? throw new InvalidOperationException($"Unable to construct options: {settingsType}");
            }
        });
}
