namespace RhoMicro.ApplicationFramework.Presentation.WebGui.App;
internal sealed class WebAppAdapter(WebApplication app) : IApplication
{
    public IServiceProvider Services => app.Services;
}