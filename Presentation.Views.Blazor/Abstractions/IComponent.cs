namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

using System.Collections.Generic;

using IMsComponent = Microsoft.AspNetCore.Components.IComponent;

/// <summary>
/// <inheritdoc/>
/// </summary>
public interface IComponent : IComponent<ICssStyle>;

/// <summary>
/// Base component providing non captured parameters via <see cref="Attributes"/> and styles via <see cref="Style"/>.
/// </summary>
/// <typeparam name="TStyle">The type of style received by this component.</typeparam>
public interface IComponent<TStyle> : IMsComponent
    where TStyle : ICssStyle
{
    /// <summary>
    /// Gets or sets the otherwise unmatched attributes passed to the component.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Required for parameter.")]
    Dictionary<String, Object?> Attributes { get; set; }
    /// <summary>
    /// Gets or sets the style to apply to the component.
    /// </summary>
    TStyle Style { get; set; }
}
