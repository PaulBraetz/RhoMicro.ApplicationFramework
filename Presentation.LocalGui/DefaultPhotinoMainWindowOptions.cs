namespace RhoMicro.ApplicationFramework.Presentation.LocalGui;

using PhotinoNET;

/// <summary>
/// Options for configuring the <see cref="PhotinoWindow"/>.
/// </summary>
public sealed class DefaultPhotinoMainWindowOptions
{
    internal void ApplyTo(PhotinoWindow window) =>
        window.SetTitle(Title)
            .SetUseOsDefaultLocation(UseOsDefaultLocation)
            .SetWidth(Width)
            .SetHeight(Height)
            .SetLeft(Left)
            .SetTop(Top);
    /// <summary>
    /// Gets or sets the default title of the <see cref="PhotinoWindow"/>.
    /// </summary>
    public String Title { get; set; } = "RhoMicro.ApplicationFramework.Presentation.LocalGui App";
    /// <summary>
    /// Gets or sets whether the <see cref="PhotinoWindow"/> uses the default os location.
    /// </summary>
    public Boolean UseOsDefaultLocation { get; set; }
    /// <summary>
    /// Gets or sets the width of the <see cref="PhotinoWindow"/>.
    /// </summary>
    public Int32 Width { get; set; }
    /// <summary>
    /// Gets or sets the height of the <see cref="PhotinoWindow"/>.
    /// </summary>
    public Int32 Height { get; set; }
    /// <summary>
    /// Gets or sets the top position of the <see cref="PhotinoWindow"/>.
    /// </summary>
    public Int32 Top { get; set; }
    /// <summary>
    /// Gets or sets the left position of the <see cref="PhotinoWindow"/>.
    /// </summary>
    public Int32 Left { get; set; }
}
