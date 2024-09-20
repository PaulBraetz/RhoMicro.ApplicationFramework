using RhoMicro.ApplicationFramework.Hosting;

using RMD.Composition;
using RMD.Presentation.WebGui.Client;

await WebServerGuiApp.CreateBuilder(
    out var builder,
    s => s.AppOptions = new() { Args = args })
    .AddBlazor()
    .ConfigureOptions(o => o.Composer += Composers.WebGui)
    .ConfigureCapabilities(c =>
    {
        _ = c.Services
            .AddRazorComponents(options => options.DetailedErrors = true)
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        _ = c.Components
            .Add(typeof(RMD.Presentation.Views.Blazor.App).Assembly)
            .Add(typeof(RMD.Presentation.WebGui.Client.EntryPoint).Assembly)
            .Add(typeof(RMD.Presentation.WebGui.Components.App).Assembly);
    })
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

        _ = app.MapRazorComponents<RMD.Presentation.WebGui.Components.App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            //map page routes
            .AddAdditionalAssemblies(
                typeof(EntryPoint).Assembly,
                typeof(RMD.Presentation.Views.Blazor.App).Assembly);
    })
    .RunAsync();
