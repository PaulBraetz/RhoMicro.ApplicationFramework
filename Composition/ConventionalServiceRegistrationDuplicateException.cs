namespace RhoMicro.ApplicationFramework.Composition;

/// <summary>
/// Thrown if duplicate service implementations have been located during conventional service registration.
/// </summary>
public sealed class ConventionalServiceRegistrationDuplicateException : Exception
{
    internal ConventionalServiceRegistrationDuplicateException(Type serviceType, IReadOnlyList<Type> implementationTypes)
        : base($"Unable to register duplicate service implementations for service {serviceType}.")
    {
        ServiceType = serviceType;
        ImplementationTypes = implementationTypes;
    }
    /// <summary>
    /// Gets the service type for which duplicate implementations were found.
    /// </summary>
    public Type ServiceType { get; }
    /// <summary>
    /// Gets the implementations found for <see cref="ServiceType"/>.
    /// </summary>
    public IReadOnlyList<Type> ImplementationTypes { get; }
}