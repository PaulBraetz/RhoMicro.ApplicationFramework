using RhoMicro.ApplicationFramework.Hosting;

using TA.Composition;
using TA.Presentation.WebGui.Client;

await WebServerGuiApp.CreateBuilder(
    out var builder,
    s => s.BuilderSettings = new() { Args = args })
    .AddBlazor()
    .AddApiServiceEndpoints()
    .ConfigureOptions(o => o.Composer += Composers.WebGui)
    .ConfigureCapabilities(c =>
    {
        _ = c.Services
            .AddRazorComponents(options => options.DetailedErrors = true)
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        _ = c.Components
            .Add(typeof(TA.Presentation.Views.Blazor.App).Assembly)
            .Add(typeof(TA.Presentation.WebGui.Client.EntryPoint).Assembly)
            .Add(typeof(TA.Presentation.WebGui.Components.App).Assembly);
    })
    .AddTimeout()
    .Build()
    .ConfigureUnderlyingApp((app, container) =>
    {
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

        _ = app.MapRazorComponents<TA.Presentation.WebGui.Components.App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            //map page routes
            .AddAdditionalAssemblies(
                typeof(EntryPoint).Assembly,
                typeof(TA.Presentation.Views.Blazor.App).Assembly);
    })
    .MapApiServiceEndpoints()
    .RunAsync()
    .ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);
