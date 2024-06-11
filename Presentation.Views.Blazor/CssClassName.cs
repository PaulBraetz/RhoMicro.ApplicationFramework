namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

using RhoMicro.CodeAnalysis;

/// <summary>
/// Represents a css class name, whose rendering via <see cref="ToString"/> is possibly conditional.
/// </summary>
[UnionType<UnconditionalCssClassName>]
[UnionType<ConditionalCssClassName>]
[UnionType<LazyCssClassName>]
[UnionTypeSettings(ToStringSetting = ToStringSetting.Simple)]
public readonly partial struct CssClassName
{
    /// <summary>
    /// Gets the length of the class name, regardless of conditionality.
    /// </summary>
    public Int32 Length => Match(
        unconditional => unconditional.AsString.Length,
        conditional => conditional.Name.AsString.Length,
        lazy => lazy.AsFactory.Invoke().AsString.Length);
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static implicit operator CssClassName(String name) => (UnconditionalCssClassName)name;
    public static implicit operator CssClassName((String name, Func<Boolean> condition) nameAndCondition) => new ConditionalCssClassName(nameAndCondition.name, nameAndCondition.condition);
    public static implicit operator CssClassName(Func<String> factory) => (LazyCssClassName)( () => (UnconditionalCssClassName)factory.Invoke() );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
