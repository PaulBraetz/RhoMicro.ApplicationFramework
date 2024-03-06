namespace RhoMicro.ApplicationFramework.Common.Abstractions;
using System;

/// <summary>
/// Represents a runtime configuration ("DEGUG", "RELEASE", etc.)
/// </summary>
public interface IEnvironmentConfiguration
{
    /// <summary>
    /// Gets the name of the configuration.
    /// </summary>
    String Name { get; }
}
