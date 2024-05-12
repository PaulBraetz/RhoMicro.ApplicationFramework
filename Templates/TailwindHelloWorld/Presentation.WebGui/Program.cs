using RhoMicro.ApplicationFramework.Hosting;

using TailwindHelloWorld.Composition;
using TailwindHelloWorld.Presentation.WebGui.Client;

await WebServerGuiApp.CreateBuilder(
    out var builder, 
    s => s.BuilderSettings = new() { Args = args })
    .AddBlazor()
    .ConfigureOptions(o => o.Composer += Composers.WebGui)
    .ConfigureCapabilities(c =>
    {
        _ = c.Services
            .AddRazorComponents(options => options.DetailedErrors = true)
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        _ = c.Components
            .Add(typeof(TailwindHelloWorld.Presentation.Views.Blazor.App).Assembly)
            .Add(typeof(TailwindHelloWorld.Presentation.WebGui.Client.EntryPoint))
            .Add(typeof(TailwindHelloWorld.Presentation.WebGui.App));
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

        _ = app.MapRazorComponents<TailwindHelloWorld.Presentation.WebGui.App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            //map page routes
            .AddAdditionalAssemblies(
                typeof(EntryPoint).Assembly,
                typeof(TailwindHelloWorld.Presentation.Views.Blazor.App).Assembly);
    })
    .RunAsync()
    .ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);
