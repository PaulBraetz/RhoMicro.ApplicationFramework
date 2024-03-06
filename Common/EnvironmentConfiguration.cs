namespace RhoMicro.ApplicationFramework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
