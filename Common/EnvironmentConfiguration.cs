namespace RhoMicro.ApplicationFramework.Common;
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
    /// Gets the debug ("DEBUG") configuration.
    /// </summary>
    public static readonly EnvironmentConfiguration Development = new("DEBUG");
    /// <summary>
    /// Gets the release ("RELEASE") configuration.
    /// </summary>
    public static readonly EnvironmentConfiguration Production = new("RELEASE");
}
