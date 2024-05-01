namespace RhoMicro.ApplicationFramework.Composition;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

/// <summary>
/// Contains composers able to compose blazor view object graphs.
/// </summary>
public static class BlazorViewComposers
{
    /// <summary>
    /// Gets the styles composer instance.
    /// </summary>
    public static IComposer CreateStylesComposer(IConfiguration configuration) =>
        Composer.Create(c =>
        {
            c.RegisterInstance<ICssStyle>(new CssStyle());

            var cssSettingsSection = configuration.GetSection("CssStyleSettings");

            var settingsTypes = typeof(CssStyleSettings).Assembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Settings", StringComparison.Ordinal) && t.IsAssignableTo(typeof(ICssStyle)));

            foreach(var settingsType in settingsTypes)
            {
                var optionsImplType = typeof(ConfigureNamedOptions<>).MakeGenericType(settingsType);
                var optionsServiceType = typeof(IOptions<>).MakeGenericType(settingsType);

                var cssStyleName = settingsType.Name[..^"Settings".Length];

                var instance = Activator.CreateInstance(optionsImplType, null, (Object settings) => cssSettingsSection.GetSection(cssStyleName).Bind(settings))
                    ?? throw new InvalidOperationException($"Unable to construct options: {settingsType}");
            }
        });
}
