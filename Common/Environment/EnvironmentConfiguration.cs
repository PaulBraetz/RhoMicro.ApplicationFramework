namespace RhoMicro.ApplicationFramework.Common.Environment;
using System;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Default implementation of <see cref="IEnvironmentConfiguration"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
public sealed class EnvironmentConfiguration : IEnvironmentConfiguration, IEquatable<IEnvironmentConfiguration>
{
    private EnvironmentConfiguration(String name) => Name = name;

    /// <inheritdoc/>
    public String Name { get; }

    /// <summary>
    /// Gets the debug ("Development") configuration.
    /// </summary>
    public static readonly EnvironmentConfiguration Development = new("Development");
    /// <summary>
    /// Gets the release ("Production") configuration.
    /// </summary>
    public static readonly EnvironmentConfiguration Production = new("Production");
    /// <summary>
    /// Gets an unknown ("Unknown") configuration.
    /// </summary>
    public static readonly EnvironmentConfiguration Unknown = new("Unknown");

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
        var name = Environment.GetEnvironmentVariable(variable);
        var result = Create(name);
#pragma warning restore RS1035 // Do not use APIs banned for analyzers

        return result;
    }
    /// <summary>
    /// Creates an <see cref="EnvironmentConfiguration"/> based on a name.
    /// </summary>
    /// <returns>
    /// A new <see cref="EnvironmentConfiguration"/>.
    /// </returns>
    public static EnvironmentConfiguration Create(String? name = null) => name is { Length: > 0 } ? new(name) : Unknown;
    /// <inheritdoc/>
    public Boolean Equals(IEnvironmentConfiguration? other) => other is { } && other.Name == Name;
}
