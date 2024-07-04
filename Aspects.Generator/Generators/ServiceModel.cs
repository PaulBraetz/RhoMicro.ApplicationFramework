namespace RhoMicro.ApplicationFramework.Aspects;

using System.Diagnostics;

using Microsoft.CodeAnalysis;

using RhoMicro.CodeAnalysis.Library;

sealed record ServiceModel(
    String RequestTypeFullName,
    EquatableList<ParameterModel> Parameters,
    ParameterModel? CancellationTokenParameter,
    Boolean ReturnsTask,
    Boolean ReturnsValueTask,
    ServiceVisibility Visibility,
    RequestTypeKind RequestTypeKind,
    String RequestTypeName,
    TypeModel ImplementationType,
    String ServiceInterfaceFullName,
    String ServiceInterfaceName,
    String Namespace,
    String ResultTypeFullName)
{
    public static ServiceModel? Create(GeneratorAttributeSyntaxContext ctx, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if(ctx.TargetSymbol is not IMethodSymbol method || !ServiceMethodAttribute.TryCreate(ctx.Attributes[0], out var attribute))
            return null;

        var implementationType = TypeModel.Create(method.ContainingType, ct);

        var requestTypeName = method.Name;
        var requestTypeFullName = attribute!.Namespace is [.., { }]
            ? $"global::{attribute!.Namespace}.{requestTypeName}"
            : $"global::{requestTypeName}";

        var serviceInterfaceName = $"I{implementationType.Signature.Name}";
        var serviceInterfaceFullName = attribute!.Namespace is [.., { }]
            ? $"global::{attribute!.Namespace}.{serviceInterfaceName}"
            : $"global::{serviceInterfaceName}";

        ParameterModel? cancellationTokenParameter = null;
        var parametersMutable = new List<ParameterModel>();

        foreach(var parameter in method.Parameters)
        {
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

        var (returnsTask, returnsValueTask, resultTypeName) = method.ReturnType switch
        {
            INamedTypeSymbol { TypeArguments: [{ } returnTypeSymbol] } =>
                getNonGenericFullyQualifiedName(method.ReturnType) switch
                {
                    Constants.TaskTypeName =>
                        (true, false, getFullyQualifiedName(returnTypeSymbol)),
                    Constants.ValueTaskTypeName =>
                        (false, true, getFullyQualifiedName(returnTypeSymbol)),
                    _ => (false, false, getFullyQualifiedName(method.ReturnType))
                },
            _ => (false, false, getFullyQualifiedName(method.ReturnType))
        };

        var result = new ServiceModel(
            ImplementationType: implementationType,
            Namespace: attribute!.Namespace,
            Visibility: attribute.Visibility,
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

        static String getFullyQualifiedName(ISymbol returnTypeSymbol) =>
            returnTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        static String getNonGenericFullyQualifiedName(ISymbol returnTypeSymbol) =>
            returnTypeSymbol.ToDisplayString(SymbolDisplayFormats.NonGenericFullyQualifiedFormat);
    }
}
