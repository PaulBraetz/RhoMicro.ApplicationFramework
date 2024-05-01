namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Exceptions;

/// <summary>
/// The <see cref="NullComponentParameterException"/> is thrown whenever a parameter is <see langword="null"/> when it shouldn't be.
/// </summary>
#pragma warning disable CA1032 // Implement standard exception constructors
public sealed class NullComponentParameterException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="parameterName">The name of the parameter that was <see langword="null"/>.</param>
    /// <param name="parameterType">The type of the parameter that was <see langword="null"/>.</param>
    /// <param name="componentType">The type of the component the exception was thrown by.</param>
    public NullComponentParameterException(String parameterName, Type parameterType, Type componentType)
        : base($"The {componentType} component requires its parameter {parameterName} of type {parameterType} to be set.")
    {
        ArgumentNullException.ThrowIfNull(parameterName);
        ArgumentNullException.ThrowIfNull(componentType);

        ParameterName = parameterName;
        ComponentType = componentType;
    }
    /// <summary>
    /// Gets the name of the parameter that was <see langword="null"/> when it shouldn't have been.
    /// </summary>
    public String ParameterName { get; }
    /// <summary>
    /// Gets the type of the component the exception was thrown by.
    /// </summary>
    public Type ComponentType { get; }
}
