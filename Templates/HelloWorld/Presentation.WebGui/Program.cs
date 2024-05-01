using RhoMicro.ApplicationFramework.Common.Environment;
using RhoMicro.ApplicationFramework.Hosting;

using HelloWorld.Composition;
using HelloWorld.Presentation.WebGui.Client;

await WebServerGuiApp.CreateBuilder(out var builder, s =>
    {
        s.BuilderSettings = new WebApplicationOptions()
        {
            Args = args,
#if DEBUG
            EnvironmentName = EnvironmentConfiguration.Development.Name
#else
            EnvironmentName = EnvironmentConfiguration.Production.Name
#endif
        };
    })
    .AddAppSettings()
    .AddConsoleLogging()
    .ConfigureOptions(o =>
    {
        o.Composer = Composers.CreateWebGui(builder.Capabilities);
    })
    .ConfigureCapabilities(c =>
    {
        _ = c.Services
            .AddRazorComponents(options =>
            {
                options.DetailedErrors = true;
            })
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        _ = c.Components
            .Add(typeof(HelloWorld.Presentation.Views.Blazor.App).Assembly)
            .Add(typeof(HelloWorld.Presentation.WebGui.Client.EntryPoint).Assembly)
            .Add(typeof(HelloWorld.Presentation.WebGui.Components.App).Assembly);
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

        _ = app.MapRazorComponents<HelloWorld.Presentation.WebGui.Components.App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            //map page routes
            .AddAdditionalAssemblies(
                typeof(EntryPoint).Assembly,
                typeof(HelloWorld.Presentation.Views.Blazor.App).Assembly);
    })
    .RunAsync()
    .ConfigureAwait(ConfigureAwaitOptions.ContinueOnCapturedContext);
