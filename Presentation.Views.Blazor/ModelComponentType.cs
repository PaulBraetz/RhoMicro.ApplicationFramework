namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;
using System;

using RhoMicro.CodeAnalysis;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;
using System.Collections.Frozen;

/// <summary>
/// Represents a <see cref="Type"/> of <see cref="IModelComponent{TModel, TStyle}"/>.
/// </summary>
[UnionType<Type>]
public readonly partial struct ModelComponentType
{
    /// <summary>
    /// Gets the set of (model type, style type) pairs used by <see cref="IModelComponent{TModel, TStyle}"/> inheritances of this type.
    /// </summary>
    public FrozenSet<ModelComponentInfo> Infos { get; private init; }
    /// <summary>
    /// Gets a value indicating whether the type provided implements <see cref="IModelComponent{TModel, TStyle}"/>.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="type"/> implements <see cref="IModelComponent{TModel, TStyle}"/>; otherwise <see langword="false"/>.
    /// </returns>
    public static Boolean IsModelComponentType(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var result = !(type.IsGenericType
            && !type.IsConstructedGenericType)
            && type.GetInterfaces()
                .Where(t =>
                {
                    if(!t.IsGenericType)
                        return false;

                    var genericDefinition = t.GetGenericTypeDefinition();
                    if(genericDefinition != typeof(IModelComponent<,>) && genericDefinition != typeof(IModelComponent<>))
                        return false;

                    return true;
                }).Any();

        return result;
    }

    private static ModelComponentType Create([UnionTypeFactory] Type type)
    {
        if(type.IsGenericType && !type.IsConstructedGenericType)
            throw new ArgumentOutOfRangeException(nameof(type), type, $"{nameof(type)} must be a constructed generic type because instances of it will be created.");

        var infos = type.GetInterfaces()
            .Where(t =>
            {
                if(!t.IsGenericType)
                    return false;

                var genericDefinition = t.GetGenericTypeDefinition();
                if(genericDefinition != typeof(IModelComponent<,>) && genericDefinition != typeof(IModelComponent<>))
                    return false;

                return true;
            })
            .Select(ModelComponentInfo.Create)
            .ToFrozenSet();

        if(infos.Count == 0)
            throw new ArgumentOutOfRangeException(nameof(type), type, $"{nameof(type)} must implement {typeof(IModelComponent<,>)} at least once.");

        return new(type)
        {
            Infos = infos
        };
    }
}

/// <summary>
/// Provides information on a <see cref="IModelComponent{TModel, TStyle}"/> type for use in dynamic components.
/// </summary>
/// <param name="ModelType">The type of model used in the type.</param>
/// <param name="StyleType">The type of style used in the type.</param>
/// <param name="HasStyle">Indicates whether the represented component type is a <see cref="IModelComponent{TModel, TStyle}"/> instead of a <see cref="IModelComponent{TModel}"/>.</param>
public readonly record struct ModelComponentInfo(Type ModelType, Type StyleType, Boolean HasStyle)
{
    internal static ModelComponentInfo Create(Type componentType)
    {
        var genericDefinition = componentType.GetGenericTypeDefinition();
        var hasStyle = genericDefinition == typeof(IModelComponent<,>);

        var genericArgs = componentType.GetGenericArguments();
        var modelType = genericArgs[0];
        var styleType = hasStyle
            ? genericArgs[1]
            : typeof(ICssStyle);

        return new(modelType, styleType, hasStyle);
    }
}