namespace RhoMicro.ApplicationFramework.Presentation.WebGui.App;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.App;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

public static class Program
{
    public static async Task Main(String[] args)
    {
        var assemblyLocation = typeof(Program).Assembly.Location;
        var currentWorkingDirectory = Path.GetDirectoryName(assemblyLocation) ?? String.Empty;
        Directory.SetCurrentDirectory(currentWorkingDirectory);

        var builder = WebApplication.CreateBuilder(args);

        var logFilePath = builder.Configuration.GetRequiredSection("LogFilePath").Value;
        _ = builder.Logging.AddFile(logFilePath);

        // Add services to the container.
        _ = builder.Services.AddRazorPages();
        _ = builder.Services.AddServerSideBlazor();
        _ = builder.Services.AddHttpContextAccessor();

        using var integrator = SimpleInjectorBlazorIntegrator.CreateDefault();

        //integrate into the app
        WebApplication app;
        try
        {
            app = AdaptIntegrate(builder, integrator);
        } catch(Exception ex)
        {
            await PrintFastExit(ex.ToString()).ConfigureAwait(false);
            return;
        }

        // Configure the HTTP request pipeline.
        if(!app.Environment.IsDevelopment())
        {
            _ = app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            _ = app.UseHsts();
        }

        _ = app.UseHttpsRedirection();
        _ = app.UseStaticFiles();
        _ = app.UseRouting();
        _ = app.MapBlazorHub();
        _ = app.MapFallbackToPage("/_Host");

        app.Run();
    }

    private static WebApplication AdaptIntegrate(WebApplicationBuilder builder, SimpleInjectorBlazorIntegrator integrator)
    {
        var adapter = new WebAppBuilderAdapter(builder);

        var result = integrator.Integrate(
           adapter,
           Root.WebGui.Compose,
           typeof(Presentation.Views.Blazor._Imports).Assembly,
           typeof().Assembly).App;

        return result;
    }

    private static async ValueTask PrintFastExit(String message)
    {
        //await integrator logs
        await Task.Delay(2500).ConfigureAwait(false);
        Console.WriteLine(String.Concat(Enumerable.Repeat('─', 100)));
        Console.WriteLine(message);
    }
}