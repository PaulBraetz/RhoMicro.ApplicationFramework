namespace RhoMicro.ApplicationFramework.Presentation.LocalGui;

using System;
using System.IO;

internal sealed class HostingEnvironment : IHostingEnvironment
{
    public const String DEFAULT_ENVIRONMENT = "Production";
    public String ContentRootPath { get; set; } = Directory.GetCurrentDirectory();
    public String EnvironmentName { get; set; } = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? DEFAULT_ENVIRONMENT;
}
