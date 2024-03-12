using RhoMicro.ApplicationFramework.Presentation.WebGui;

using HelloWorld.Presentation.Composition;
using HelloWorld.Presentation.WebGui.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
_ = builder.Services
    .AddLogging(loggingBuilder => loggingBuilder.AddConsole())
    .AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.IntegrateSimpleInjectorWeb(
    Composers.WebGui,
    [typeof(EntryPoint).Assembly, typeof(HelloWorld.Presentation.Views.Blazor.App).Assembly, typeof(HelloWorld.Presentation.WebGui.Components.App).Assembly],
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

_ = app.MapRazorComponents<HelloWorld.Presentation.WebGui.Components.App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(EntryPoint).Assembly,
        typeof(HelloWorld.Presentation.Views.Blazor.App).Assembly);

app.Run();
