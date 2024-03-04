namespace RhoMicro.ApplicationFramework.Presentation.WebGui;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

internal sealed class WebAssemblyHostBuilderAdapter(WebAssemblyHostBuilder builder) : IApplicationBuilder<WebAssemblyHostAdapter>
{
    public IServiceCollection Services => builder.Services;
    public WebAssemblyHostAdapter Build() => new(builder.Build());
}
