namespace RhoMicro.ApplicationFramework.Aspects;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using RhoMicro.CodeAnalysis.Library.Text;
using RhoMicro.CodeAnalysis.Generated;

using static RhoMicro.CodeAnalysis.Library.Text.IndentedStringBuilder.Appendables;

/// <summary>
/// Generates required members for AOP annotated service methods and request types.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class Generator : IIncrementalGenerator
{
    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        //if(!Debugger.Launch())
        //    Debugger.Break();

        var settingsProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
                typeof(ServiceSettingsAttribute).FullName,
                (node, ct) => true,
                SettingsModel.Create)
            .Where(m => m is not null)
            .Collect()
            .Select((settings, ct) =>
            {
                ct.ThrowIfCancellationRequested();
                var result = settings is []
                    ? SettingsModel.Default
                    : settings[0];

                return result;
            });

        var servicesProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
                typeof(ServiceMethodAttribute).FullName,
                (node, ct) => node is MethodDeclarationSyntax,
                ServiceModel.Create)
            .Where(m => m is not null);

        var provider = servicesProvider!
            .Combine(settingsProvider!)
            .Select((t, ct) =>
            {
                ct.ThrowIfCancellationRequested();

                var (serviceModel, settingsModel) = t;

                var builder = new IndentedStringBuilder(
                    IndentedStringBuilderOptions.GeneratedFile with
                    {
                        AmbientCancellationToken = ct,
                        GeneratorName = typeof(Generator).FullName
                    });

                _ = builder.Operators +
                Append(b =>
                {
                    if(serviceModel!.Namespace is not [.., { }])
                        return;

                    _ = b.Append("namespace ").Append(serviceModel!.Namespace).OpenBracesBlock();
                }) +
                    Append(b =>
                    {
                        if(serviceModel!.RequestTypeKind is RequestTypeKind.MatchParameterType)
                            return;

                        _ = b.Operators +
                        Append(serviceModel.Visibility) + ' ' +
                        Append(serviceModel.RequestTypeKind) + ' ' +
                        serviceModel.RequestTypeName + '(' +
                            Append(b =>
                            {
                                for(var i = 0; i < serviceModel.Parameters.Count; i++)
                                {
                                    if(i > 0)
                                        b.AppendCore(',');

                                    var paramModel = serviceModel.Parameters[i];
                                    b.Append(paramModel.Type).Append(' ').AppendCore(paramModel.PropertyName);
                                }
                            }) + ')' +
                            " : " + settingsModel!.RequestInterfaceTypeName + '<' + serviceModel.ResultTypeFullName + '>' + ';' + NewLine;
                    }) +
                    Append(serviceModel!.Visibility) + " interface " + serviceModel.ServiceInterfaceName +
                    OpenBracesBlock() +
                        AppendImplementationSignature(serviceModel) + ';' +
                    CloseBlock() +
                CloseAllBlocks() +
                Append(b =>
                {
                    if(serviceModel.ImplementationType.Namespace is not [.., { }])
                        return;

                    _ = b.Append("namespace ").Append(serviceModel.ImplementationType.Namespace).OpenBracesBlock();
                }) +
                    Append(b =>
                    {
                        var signatures = new Stack<TypeSignatureModel>();
                        var implTypeSig = serviceModel.ImplementationType.Signature;
                        var signature = implTypeSig.ParentType;
                        while(signature is not null)
                        {
                            signatures.Push(signature);
                            signature = signature.ParentType;
                        }

                        foreach(var sig in signatures)
                        {
                            _ = b.Operators +
                            "partial " + sig.RecordKeyword + ' ' + sig.TypeKind + sig.Name + OpenBracesBlock();
                        }

                        _ = b.Operators +
                        '[' + settingsModel!.ServiceInjectionInfoAttributeTypeName +
                        '(' + NewLine +
                        Indent() +
                            "RequestType = typeof(" + serviceModel.RequestTypeFullName + ")," + NewLine +
                            "ResultType = typeof(" + serviceModel.ResultTypeFullName + ")," + NewLine +
                            "ServiceType = typeof(" + serviceModel.ServiceInterfaceFullName + ")," + NewLine +
                            "ImplementationType = typeof(" + serviceModel.ImplementationType.Signature.FullDisplayName + ")," + NewLine +
                            "AdapterType = typeof(" + serviceModel.ImplementationType.Signature.Name + "Adapter" + NewLine +
                        Detent() + NewLine +
                        "))]" + NewLine +
                        "partial " + implTypeSig.RecordKeyword + ' ' + implTypeSig.TypeKind + ' ' + implTypeSig.Name +
                        " : " + AppendServiceType(serviceModel, settingsModel) +
                        OpenBracesBlock() +
                            "global::System.Threading.Tasks.ValueTask<" + serviceModel.ResultTypeFullName + "> " +
                            AppendServiceType(serviceModel, settingsModel) + '.' + settingsModel.ExecuteName + '(' +
                            serviceModel.RequestTypeFullName + " request, " + Constants.CtTypeName + " cancellationToken) =>" + NewLine +
                            Indent() +
                                Append(b =>
                                {
                                    if(!serviceModel.ReturnsValueTask)
                                    {
                                        b.Append("new ").Append(Constants.ValueTaskTypeName).Append('<').Append(serviceModel.ResultTypeFullName).AppendCore(">(");
                                    }

                                    _ = b.AppendAdapterInvocation(serviceModel);

                                    if(!serviceModel.ReturnsValueTask)
                                    {
                                        b.AppendCore(')');
                                    }

                                    b.AppendCore(';');
                                }) +
                            Detent() +
                        CloseBlock();
                    }) +
                CloseAllBlocks() +
                "file sealed class " + serviceModel.ImplementationType.Signature.Name + "Adapter(" + AppendServiceType(serviceModel, settingsModel!) + " service) : " + serviceModel.ServiceInterfaceFullName +
                OpenBracesBlock() +
                    "public " + AppendImplementationSignature(serviceModel) + " =>" + NewLine +
                    Indent() +
                        "service." + settingsModel!.ExecuteName + "(new " + serviceModel.RequestTypeFullName + '(' + Append(b =>
                        {
                            if(serviceModel.Parameters.Count == 0)
                                return;

                            var firstParam = serviceModel.Parameters[0];
                            b.Append(firstParam.PropertyName).Append(": ").AppendCore(firstParam.Name);

                            for(var i = 1; i < serviceModel.Parameters.Count; i++)
                            {
                                var param = serviceModel.Parameters[i];
                                b.Append(", ").Append(param.PropertyName).Append(": ").AppendCore(param.Name);
                            }
                        }) + "), cancellationToken);" +
                    Detent() +
                CloseBlock();

                var source = builder.ToString();
                var hintName = $"{serviceModel.ImplementationType.Signature.HintName}_{source.GetHashCode()}";

                return (hintName, source);
            });

        IncludedFileSources.RegisterToContext(context);
        context.RegisterSourceOutput(provider, (ctx, t) => ctx.AddSource(t.hintName, t.source));
    }
}
