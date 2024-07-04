namespace RhoMicro.ApplicationFramework.Aspects;

using System.Diagnostics.CodeAnalysis;

using Microsoft.CodeAnalysis;

sealed record SettingsModel(
    String RequestInterfaceTypeName,
    String ServiceInterfaceTypeName,
    String ExecuteName,
    String ServiceInjectionInfoAttributeTypeName)
{
    public static SettingsModel Default { get; } = new(
            RequestInterfaceTypeName: Constants.DefaultRequestInterfaceTypeName,
            ServiceInterfaceTypeName: Constants.DefaultServiceInterfaceTypeName,
            ExecuteName: Constants.DefaultExecuteName,
            ServiceInjectionInfoAttributeTypeName: Constants.DefaultServiceInjectionInfoAttributeType);

    static Boolean TryGetRequestInterfaceTypeName(ServiceSettingsAttribute attribute, [NotNullWhen(true)] out String? requestInterfaceTypeName)
    {
        requestInterfaceTypeName = null;

        if(attribute is
            {
                RequestInterfaceTypeSymbol:
                {
                    TypeParameters: [{ }],
                } requestInterfaceTypeSymbol
            } && requestInterfaceTypeSymbol.OriginalDefinition.MemberNames.Any())
        {
            requestInterfaceTypeName = requestInterfaceTypeSymbol.ToDisplayString(SymbolDisplayFormats.NonGenericFullyQualifiedFormat);
        } else if(attribute is { RequestInterfaceTypeSymbol: null })
        {
            requestInterfaceTypeName = Default.RequestInterfaceTypeName;
        }

        return requestInterfaceTypeName is not null;
    }
    static Boolean TryGetServiceInjectionInfoAttributeTypeName(ServiceSettingsAttribute attribute, [NotNullWhen(true)] out String? serviceInjectionInfoAttributeTypeName)
    {
        serviceInjectionInfoAttributeTypeName = null;

        if(attribute is
            {
                ServiceInjectionInfoAttributeTypeSymbol:
                {
                    TypeParameters: []
                } serviceInjectionInfoAttributeTypeSymbol
            })
        {
            serviceInjectionInfoAttributeTypeName = serviceInjectionInfoAttributeTypeSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        } else if(attribute is { ServiceInjectionInfoAttributeTypeSymbol: null })
        {
            serviceInjectionInfoAttributeTypeName = Default.ServiceInjectionInfoAttributeTypeName;
        }

        return serviceInjectionInfoAttributeTypeName is not null;
    }
    static Boolean TryGetServiceInterfaceTypeSymbolAndExecuteName(ServiceSettingsAttribute attribute, [NotNullWhen(true)] out String? serviceInterfaceTypeName, [NotNullWhen(true)] out String? executeName)
    {
        serviceInterfaceTypeName = null;
        executeName = null;

        if(attribute is
            {
                ServiceInterfaceTypeSymbol:
                {
                    TypeParameters: [{ } requestType, { } resultType]
                } serviceInterfaceTypeSymbol
            }
            && serviceInterfaceTypeSymbol.OriginalDefinition.GetMembers() is [
               IMethodSymbol
            {
                Parameters: [{ Type: { } firstParamType }, { Type: { } secondParamType }],
                ReturnType: INamedTypeSymbol { TypeArguments: [{ } returnTypeArg] } returnType,
                Name: { } name
            }]
           && returnType.ToDisplayString(SymbolDisplayFormats.NonGenericFullyQualifiedFormat) == Constants.ValueTaskTypeName
           && secondParamType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == Constants.CtTypeName
           && SymbolEqualityComparer.IncludeNullability.Equals(firstParamType.OriginalDefinition, requestType)
           && SymbolEqualityComparer.IncludeNullability.Equals(returnTypeArg.OriginalDefinition, resultType))
        {
            serviceInterfaceTypeName = serviceInterfaceTypeSymbol.ToDisplayString(SymbolDisplayFormats.NonGenericFullyQualifiedFormat);
            executeName = name;
        } else if(attribute is { ServiceInterfaceTypeSymbol: null })
        {
            serviceInterfaceTypeName = Default.ServiceInterfaceTypeName;
            executeName = Default.ExecuteName;
        }

        return serviceInterfaceTypeName is not null;
    }
    public static SettingsModel? Create(GeneratorAttributeSyntaxContext ctx, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if(!ServiceSettingsAttribute.TryCreate(ctx.Attributes[0], out var attribute))
            return null;

        if(!TryGetRequestInterfaceTypeName(attribute!, out var requestInterfaceTypeName)
           || !TryGetServiceInjectionInfoAttributeTypeName(attribute!, out var serviceInjectionInfoAttributeTypeName)
           || !TryGetServiceInterfaceTypeSymbolAndExecuteName(attribute!, out var serviceInterfaceTypeName, out var executeName))
        {
            return null;
        }

        var result = new SettingsModel(
            RequestInterfaceTypeName: requestInterfaceTypeName,
            ServiceInterfaceTypeName: serviceInterfaceTypeName,
            ExecuteName: executeName,
            ServiceInjectionInfoAttributeTypeName: serviceInjectionInfoAttributeTypeName);

        return result;
    }
}
