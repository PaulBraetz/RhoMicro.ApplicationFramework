namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;

/// <summary>
/// Represents a conditional css class name, whose rendering as a <see cref="String"/> via <see cref="ToString"/> depends on a condition.
/// </summary>
/// <param name="Name">The name of the css class.</param>
/// <param name="Condition">The condition to query upon rendering via <see cref="ToString"/>.</param>
public readonly record struct ConditionalCssClassName(UnconditionalCssClassName Name, Func<Boolean> Condition)
{
    /// <inheritdoc/>
    public override String ToString() =>
        Condition.Invoke()
        ? Name
        : String.Empty;
}
