namespace RhoMicro.ApplicationFramework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Default implementation of <see cref="IAspEnvironment"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="name">The name of the runtime configuration.</param>
public sealed class AspEnvironment(String name) : IAspEnvironment
{
    /// <inheritdoc/>
    public String Name { get; } = name;
    /// <summary>
    /// Gets the debug ("DEBUG") configuration.
    /// </summary>
    public static readonly AspEnvironment Development = new("DEBUG");
    /// <summary>
    /// Gets the release ("RELEASE") configuration.
    /// </summary>
    public static readonly AspEnvironment Production = new("RELEASE");
}
