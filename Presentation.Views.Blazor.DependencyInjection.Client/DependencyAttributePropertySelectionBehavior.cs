namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection.Client;

using System.Reflection;

using SimpleInjector.Advanced;

internal sealed class DependencyAttributePropertySelectionBehavior : IPropertySelectionBehavior
{
    public Boolean SelectProperty(Type type, PropertyInfo prop)
    {
        var result = prop.GetCustomAttribute<InjectedAttribute>() != null;

        return result;
    }
}
