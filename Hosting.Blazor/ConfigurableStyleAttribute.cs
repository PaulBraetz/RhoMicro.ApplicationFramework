namespace RhoMicro.ApplicationFramework.Hosting;

/// <summary>
/// Marks the target component to use configurable styles. Add this attribute to components to map their styles in the app configurations.
/// </summary>
/// <typeparam name="TStyle">The type of style to apply to the component.</typeparam>
/// <typeparam name="TStyleSettings">The type of mutable settings implementing <see cref="TStyle"/> for use via the options pattern.</typeparam>
public sealed class ConfigurableStyleAttribute<TStyle, TStyleSettings>
    : ConfigurableStyleAttribute
    where TStyleSettings : class, TStyle
    where TStyle : class
{
    internal override Type StyleSettingsType => typeof(TStyleSettings);
    internal override Type StyleType => typeof(TStyle);
}
