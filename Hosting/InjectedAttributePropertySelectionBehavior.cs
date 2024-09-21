namespace RhoMicro.ApplicationFramework.Hosting;
using System.Reflection;

using RhoMicro.ApplicationFramework.Composition;

using SimpleInjector.Advanced;

sealed class InjectedAttributePropertySelectionBehavior : IPropertySelectionBehavior
{
    public Boolean SelectProperty(Type type, PropertyInfo prop)
    {
        var result = prop.GetCustomAttribute<InjectedAttribute>() != null;

        return result;
    }
}
