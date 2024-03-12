namespace RhoMicro.ApplicationFramework.Presentation.WebGui;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

internal sealed class WebApplicationBuilderAdapter(WebApplicationBuilder builder) : IApplicationBuilder<WebApplicationAdapter>
{
    public IServiceCollection Services => builder.Services;
    public WebApplicationAdapter Build() => new(builder.Build());
}
