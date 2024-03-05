namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Abstractions;

using Microsoft.AspNetCore.Components;

using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;
using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.Exceptions;

using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

/// <summary>
/// <inheritdoc/>
/// </summary>
public abstract class ComponentBase : ComponentBase<ICssStyle>;

/// <summary>
/// Base component providing non captured parameters via <see cref="Attributes"/>
/// as well as providing automated checking for required parameters in <see cref="OnParametersSet"/>.
/// Required parameters must be annotated using the <see cref="ParameterAttribute"/> and the <see cref="RequiredAttribute"/>.
/// Upon encountering a required parameter with value <see langword="null"/>,
/// a <see cref="ParameterNullException"/> will be thrown.
/// </summary>
/// <typeparam name="TStyle">The type of style received by this component.</typeparam>
#pragma warning disable CA1063 // Implement IDisposable Correctly
public abstract class ComponentBase<TStyle> : SimpleInjectorIntegratedComponent, IDisposable
    where TStyle : ICssStyle
#pragma warning restore CA1063 // Implement IDisposable Correctly
{
    private static readonly ConcurrentDictionary<Type, IReadOnlyList<Action<Object>>> _nullChecks = new();
    private readonly CancellationTokenSource _disposalCts = new();
    private Int32 _disposed = BooleanState.FalseState;
    private Dictionary<String, Object?>? _attributes;

    /// <summary>
    /// Gets or sets the style to apply to the component.
    /// </summary>
    [Injected]
    [Parameter]
    public required TStyle Style { get; set; }

    private Dictionary<String, Object?> AttributesInternal
    {
        get
        {
            if(_attributes == null)
            {
                _attributes = [];
                EnsureComponentTypeAttributes();
            }

            return _attributes;
        }
    }

    /// <summary>
    /// Gets or sets the otherwise unmatched attributes passed to the component.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Required for parameter.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "BL0007:Component parameters should be auto properties", Justification = "Impossible due to EnsureComponentType")]
    public required Dictionary<String, Object?> Attributes
    {
        get => AttributesInternal;
        set
        {
            _attributes = value;
            EnsureComponentTypeAttributes();
        }
    }
    /// <summary>
    /// Gets a cancellation token that will be cancelled upon <see cref="Dispose"/> being called.
    /// </summary>
    protected CancellationToken ComponentDisposalToken => _disposalCts.Token;

    private void EnsureComponentTypeAttributes()
    {
        if(!AspEnvironment.IsDevelopment())
            return;

        _ = Attributes.TryAdd("component-type", ComponentType.FullName ?? String.Empty);
    }
    /// <summary>
    /// Ensures that each of the class names provided are removed from the <c>class</c> attribute.
    /// </summary>
    /// <param name="classNames">
    /// The class names ensure are removed from the <c>class</c> attribute.
    /// </param>
    protected void RemoveClassNames(params String[] classNames) =>
        ConfigureClassNames([], classNames);
    /// <summary>
    /// Ensures that each of the class names provided are removed from the <c>class</c> attribute.
    /// </summary>
    /// <param name="classNames">
    /// The class names ensure are removed from the <c>class</c> attribute.
    /// </param>
    protected void RemoveClassNames(IEnumerable<String> classNames) =>
        ConfigureClassNames([], classNames);
    /// <summary>
    /// Ensures that each of the class names provided are present in the <c>class</c> attribute.
    /// </summary>
    /// <param name="classNames">
    /// The class names ensure are present in the <c>class</c> attribute.
    /// </param>
    protected void EnsureClassNames(IEnumerable<String> classNames) =>
        ConfigureClassNames(classNames, []);
    /// <summary>
    /// Ensures that each of the class names provided are present in the <c>class</c> attribute.
    /// </summary>
    /// <param name="classNames">
    /// The class names ensure are present in the <c>class</c> attribute.
    /// </param>
    protected void EnsureClassNames(params String[] classNames) =>
        ConfigureClassNames(classNames, []);
    /// <summary>
    /// Configures the class names available in the <c>class</c> attribute.
    /// </summary>
    /// <param name="ensure">The class names to ensure.</param>
    /// <param name="remove">The class names to ensure are not available.</param>
    protected void ConfigureClassNames(IEnumerable<String> ensure, IEnumerable<String> remove)
    {
        var removeSet = remove
            .Select(n => n.Trim())
            .ToHashSet();

        var trimmedClassNames = ensure
            .Select(n => n.Trim())
            .Where(n => !removeSet.Contains(n));

        if(!Attributes.TryGetValue("class", out var existingClassNames) ||
            existingClassNames == null ||
            existingClassNames is not String)
        {
            Attributes["class"] = String.Join(' ', trimmedClassNames);
            return;
        }

        var classSet = ( existingClassNames as String )!
            .Split(' ', StringSplitOptions.TrimEntries)
            .Where(n => !removeSet.Contains(n))
            .ToHashSet();

        foreach(var className in trimmedClassNames)
        {
            _ = classSet.Add(className);
        }

        var newClassNames = String.Join(' ', classSet);
        Attributes["class"] = newClassNames;
    }
    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        CheckNullParameters();
        _ = Attributes.TryAdd("class", Style.GetCssClass());
    }

    private void CheckNullParameters()
    {
        var nullChecks = GetNullChecksForComponent();
        foreach(var nullCheck in nullChecks)
        {
            nullCheck.Invoke(this);
        }
    }

    private IReadOnlyList<Action<Object>> GetNullChecksForComponent()
    {
        var componentType = GetType();
        var result = _nullChecks.GetOrAdd(componentType, CreateNullChecksForComponent);

        return result;
    }
    private static IReadOnlyList<Action<Object>> CreateNullChecksForComponent(Type componentType)
    {
        var result = componentType
            .GetProperties()
            .Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(ParameterAttribute)))
            //only check parameters with required keyword
            .Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(RequiredMemberAttribute)))
            .Where(p => !p.PropertyType.IsValueType || Nullable.GetUnderlyingType(p.PropertyType) != null)
            .Select(CreateNullCheck)
            .ToList();

        return result;
    }
    private static Action<Object> CreateNullCheck(PropertyInfo info)
    {
        var thisType = info.DeclaringType
            ?? throw new ArgumentException($"Unable to obtain declaring type of {info}.", nameof(info));
        //instance
        var instanceParam = Expression.Parameter(typeof(Object));
        //(Type)instance
        var castExpr = Expression.Convert(instanceParam, thisType);
        //((Type)instance).Prop
        var propertyExpr = Expression.Property(castExpr, info);
        //null
        var nullExpr = Expression.Constant(null, info.PropertyType);
        //((Type)instance).Prop == null
        var equalityExpr = Expression.Equal(propertyExpr, nullExpr);
        //"Prop"
        var paramNameExpr = Expression.Constant(info.Name);
        //Type
        var thisTypeExpr = Expression.Constant(thisType);
        //Type
        var propertyTypeExpr = Expression.Constant(info.PropertyType);
        var exceptionCtor = typeof(ParameterNullException).GetConstructor([typeof(String), typeof(Type), typeof(Type)])!;
        //new ParameterNullException("Prop", Type, Type)
        var exceptionExpr = Expression.New(exceptionCtor, paramNameExpr, propertyTypeExpr, thisTypeExpr);
        //throw new ParameterNullException("Prop", Type, Type)
        var throwExpr = Expression.Throw(exceptionExpr);
        //void
        var voidExpr = Expression.Constant(typeof(void));
        //if(((Type)instance).Prop == null)
        //	throw new ParameterNullException("Prop", Type, Type)
        //else
        //	void
        var ifThenExpr = Expression.IfThenElse(equalityExpr, throwExpr, voidExpr);

        //(Object instance)=>{
        //if(((Type)instance).Prop == null)
        //		throw new ParameterNullException("Prop", Type, Type)
        //void (return)
        //}
        var lambdaExpr = Expression.Lambda(ifThenExpr, instanceParam);

        var result = (Action<Object>)lambdaExpr.Compile();

        return result;
    }

    /// <summary>
    /// Called the first time the instance is disposed.
    /// </summary>
    protected virtual void OnDisposing() { }

    /// <inheritdoc/>
#pragma warning disable CA1063 // Implement IDisposable Correctly
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
#pragma warning restore CA1063 // Implement IDisposable Correctly
    {
        if(Interlocked.Exchange(
            ref _disposed,
            BooleanState.TrueState) == BooleanState.FalseState)
        {
            _disposalCts.Cancel();
            _disposalCts.Dispose();
            OnDisposing();
        }
    }
}
