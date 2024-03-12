#pragma warning disable CA1848 // Use the LoggerMessage delegates
namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Logging;

using Microsoft.Extensions.Logging;

using SimpleInjector;
using SimpleInjector.Diagnostics;

/// <summary>
/// Logs diagnostics provided by the container.
/// </summary>
public sealed class ContainerDiagnosticsLogger : IContainerLogger
{
    /// <inheritdoc/>
    public void Log(Container container, ILogger logger)
    {
        var analysisResults = Analyzer.Analyze(container);
        foreach(var result in analysisResults)
        {
            switch(result.Severity)
            {
                case DiagnosticSeverity.Warning:
                    logger.LogWarning("Injection warning: {Diagnostic}", result.Description);
                    break;
                case DiagnosticSeverity.Information:
                    logger.LogWarning("Injection info: {Diagnostic}", result.Description);
                    break;
            }
        }
    }
}
