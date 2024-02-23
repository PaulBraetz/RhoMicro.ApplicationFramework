namespace RhoMicro.ApplicationFramework.Presentation.WebGui.App;
internal sealed class WebAppBuilderAdapter(WebApplicationBuilder builder) : IApplicationBuilder<WebAppAdapter>
{
    public IServiceCollection Services => builder.Services;

    public WebAppAdapter Build()
    {
        var app = builder.Build();
        var result = new WebAppAdapter(app);

        return result;
    }
}