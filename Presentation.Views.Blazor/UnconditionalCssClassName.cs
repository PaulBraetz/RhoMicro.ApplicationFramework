namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;
using RhoMicro.CodeAnalysis;

/// <summary>
/// Represents an unconditional css class name, whose rendering does not depend on any conditions.
/// </summary>
[UnionType<String>]
[UnionTypeSettings(ToStringSetting = ToStringSetting.Simple)]
public readonly partial struct UnconditionalCssClassName
{
    private static UnconditionalCssClassName Create([UnionTypeFactory] String value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var trimmedValue = value.Trim(' ');

        return new(trimmedValue);
    }
}
