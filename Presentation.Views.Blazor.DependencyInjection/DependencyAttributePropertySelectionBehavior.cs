namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;
using System.Reflection;

using SimpleInjector.Advanced;

internal sealed class DependencyAttributePropertySelectionBehavior : IPropertySelectionBehavior
{
    public Boolean SelectProperty(Type type, PropertyInfo prop)
    {
        var result = prop.GetCustomAttributes(typeof(InjectedAttribute)).Any();

        return result;
    }
}
