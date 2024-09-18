namespace RhoMicro.ApplicationFramework.Aspects;

using Microsoft.CodeAnalysis;

using RhoMicro.CodeAnalysis.Library;

sealed record ServiceModel(
    Boolean IsExternal,
    String RequestTypeFullName,
    EquatableList<ParameterModel> Parameters,
    ParameterModel? CancellationTokenParameter,
    Boolean ReturnsTask,
    Boolean ReturnsValueTask,
    ServiceVisibility? Visibility,
    RequestTypeKind RequestTypeKind,
    String RequestTypeName,
    TypeModel ImplementationType,
    String ServiceInterfaceFullName,
    String ServiceInterfaceName,
    String Namespace,
    String ResultTypeFullName)
{
    public static ServiceModel? CreateFromPartial(GeneratorAttributeSyntaxContext ctx, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if(ctx.TargetSymbol is not IMethodSymbol method ||
           !ServiceMethodImplementationAttribute.TryCreate(ctx.Attributes[0], out var attribute) ||
           //!attribute!.RequestSymbol.IsRecord ||
           attribute!.ServiceSymbol.TypeKind != TypeKind.Interface)
        {
            return null;
        }

        var requestTypeName = attribute.RequestSymbol.Name;
        var requestTypeFullName = attribute.RequestSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        var serviceInterfaceName = attribute.ServiceSymbol.Name;
        var serviceInterfaceFullName = attribute.ServiceSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        var (parameters, cancellationTokenParameter) = GetParameters(method, ct);

        var (returnsTask, returnsValueTask, resultTypeName) = InterpretReturnType(method, ct);

        var implementationType = TypeModel.Create(method.ContainingType, ct);

        var @namespace = implementationType.Namespace;

        ServiceVisibility? visibility = null;

        var requestTypeKind = method.ContainingType switch
        {
            { IsRecord: true, TypeKind: TypeKind.Struct } => RequestTypeKind.RecordStruct,
            { IsRecord: true, TypeKind: TypeKind.Class } => method.ContainingType switch
            {
                { IsSealed: true } => RequestTypeKind.SealedRecordClass,
                { IsAbstract: true } => RequestTypeKind.AbstractRecordClass,
                _ => RequestTypeKind.RecordClass
            },
            _ => default
        };

        var result = new ServiceModel(
            IsExternal: true,
            ImplementationType: implementationType,
            Namespace: @namespace,
            Visibility: visibility,
            RequestTypeName: requestTypeName,
            RequestTypeFullName: requestTypeFullName,
            RequestTypeKind: requestTypeKind,
            Parameters: parameters,
            ResultTypeFullName: resultTypeName,
            ServiceInterfaceFullName: serviceInterfaceFullName,
            ServiceInterfaceName: serviceInterfaceName,
            ReturnsTask: returnsTask,
            ReturnsValueTask: returnsValueTask,
            CancellationTokenParameter: cancellationTokenParameter);

        return result;
    }
    public static ServiceModel? Create(GeneratorAttributeSyntaxContext ctx, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if(ctx.TargetSymbol is not IMethodSymbol method || !ServiceMethodAttribute.TryCreate(ctx.Attributes[0], out var attribute))
            return null;

        var implementationType = TypeModel.Create(method.ContainingType, ct);

        var @namespace = attribute!.Namespace ?? implementationType.Namespace;

        var requestTypeName = method.Name;
        var requestTypeFullName = @namespace is []
            ? $"global::{requestTypeName}"
            : $"global::{@namespace}.{requestTypeName}";

        var serviceInterfaceName = attribute.ServiceInterfaceName ?? $"I{method.ContainingType.Name}";
        var serviceInterfaceFullName = @namespace is [.., { }]
            ? $"global::{@namespace}.{serviceInterfaceName}"
            : $"global::{serviceInterfaceName}";

        var (parameters, cancellationTokenParameter) = GetParameters(method, ct);
        var (returnsTask, returnsValueTask, resultTypeName) = InterpretReturnType(method, ct);

        ServiceVisibility? visibility = attribute.Visibility is ServiceVisibility.Default
            ? null
            : attribute.Visibility;

        var result = new ServiceModel(
            IsExternal: false,
            ImplementationType: implementationType,
            Namespace: @namespace,
            Visibility: visibility,
            RequestTypeName: requestTypeName,
            RequestTypeFullName: requestTypeFullName,
            RequestTypeKind: attribute.RequestTypeKind,
            Parameters: parameters,
            ResultTypeFullName: resultTypeName,
            ServiceInterfaceFullName: serviceInterfaceFullName,
            ServiceInterfaceName: serviceInterfaceName,
            ReturnsTask: returnsTask,
            ReturnsValueTask: returnsValueTask,
            CancellationTokenParameter: cancellationTokenParameter);

        return result;
    }
    private static (EquatableList<ParameterModel> parameters, ParameterModel? cancellationTokenParameter) GetParameters(IMethodSymbol method, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        ParameterModel? cancellationTokenParameter = null;
        var parametersMutable = new List<ParameterModel>();

        foreach(var parameter in method.Parameters)
        {
            ct.ThrowIfCancellationRequested();

            var model = ParameterModel.Create(parameter);
            if(parameter.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == Constants.CtTypeName)
            {
                if(cancellationTokenParameter.HasValue)
                {
                    parametersMutable.Add(cancellationTokenParameter.Value);
                }

                cancellationTokenParameter = model;
            } else
            {
                parametersMutable.Add(model);
            }
        }

        var parameters = parametersMutable.AsEquatable();

        return (parameters, cancellationTokenParameter);
    }
    private static (Boolean returnsTask, Boolean returnsValueTask, String resultTypeName) InterpretReturnType(IMethodSymbol method, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var result = method.ReturnType switch
        {
            INamedTypeSymbol { TypeArguments: [{ } returnTypeSymbol] } =>
                GetNonGenericFullyQualifiedName(method.ReturnType) switch
                {
                    Constants.TaskTypeName =>
                        (true, false, GetFullyQualifiedName(returnTypeSymbol)),
                    Constants.ValueTaskTypeName =>
                        (false, true, GetFullyQualifiedName(returnTypeSymbol)),
                    _ => (false, false, GetFullyQualifiedName(method.ReturnType))
                },
            _ => (false, false, GetFullyQualifiedName(method.ReturnType))
        };

        return result;
    }

    static String GetFullyQualifiedName(ISymbol returnTypeSymbol) =>
        returnTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
    static String GetNonGenericFullyQualifiedName(ISymbol returnTypeSymbol) =>
        returnTypeSymbol.ToDisplayString(SymbolDisplayFormats.NonGenericFullyQualifiedFormat);
}
