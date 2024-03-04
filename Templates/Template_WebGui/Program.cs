namespace Template_WebGui;

using RhoMicro.ApplicationFramework.Presentation.WebGui;

using RhoMicro.ApplicationFramework.Composition;

using Template_WebGui.Client;
using RhoMicro.ApplicationFramework.Template_Composition;

public class Program
{
    public static void Main(String[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        _ = builder.Services
            .AddLogging(loggingBuilder => loggingBuilder.AddConsole())
            .AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        var app = builder.IntegrateSimpleInjectorWeb(
            TemplateComposers.WebGui,
            [typeof(EntryPoint).Assembly, typeof(Components.App).Assembly, typeof(Template_Views.App).Assembly],
            out var containerLifetime);

        using var _0 = containerLifetime;

        // Configure the HTTP request pipeline.
        if(app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        } else
        {
            _ = app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            _ = app.UseHsts();
        }

        _ = app.UseHttpsRedirection();

        _ = app.UseStaticFiles();
        _ = app.UseAntiforgery();

        _ = app.MapRazorComponents<Components.App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(
                typeof(EntryPoint).Assembly,
                typeof(Template_Views.App).Assembly);

        app.Run();
    }
}
