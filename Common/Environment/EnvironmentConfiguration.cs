namespace RhoMicro.ApplicationFramework.Common.Environment;
using System;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Default implementation of <see cref="IEnvironmentConfiguration"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="Name">The name of the runtime configuration.</param>
public sealed record EnvironmentConfiguration(String Name) : IEnvironmentConfiguration
{
    /// <summary>
    /// Gets the debug ("Development") configuration.
    /// </summary>
    public static readonly EnvironmentConfiguration Development = new("Development");
    /// <summary>
    /// Gets the release ("Production") configuration.
    /// </summary>
    public static readonly EnvironmentConfiguration Production = new("Production");
    /// <summary>
    /// Creates an <see cref="EnvironmentConfiguration"/> based on an environment variable.
    /// </summary>
    /// <returns>
    /// A new <see cref="EnvironmentConfiguration"/>.
    /// </returns>
    public static EnvironmentConfiguration CreateFromEnvironmentVariable(String variable = "DOTNET_ENVIRONMENT")
    {
        ArgumentNullException.ThrowIfNull(variable);

#pragma warning disable RS1035 // Do not use APIs banned for analyzers
        var name = Environment.GetEnvironmentVariable(variable) ?? Production.Name;
#pragma warning restore RS1035 // Do not use APIs banned for analyzers

        return new(name);
    }
}
