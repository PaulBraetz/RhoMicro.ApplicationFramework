namespace RhoMicro.ApplicationFramework.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Represents the runtime configuration ("DEGUG", "RELEASE", etc.)
/// </summary>
public interface IAspEnvironment
{
    /// <summary>
    /// Gets the name of the configuration.
    /// </summary>
    String Name { get; }
}

