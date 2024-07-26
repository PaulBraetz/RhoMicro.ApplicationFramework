namespace RhoMicro.ApplicationFramework.Aspects;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using RhoMicro.CodeAnalysis.Library.Text;
using RhoMicro.CodeAnalysis.Library;
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
                (node, ct) => true,
                ServiceModel.Create)
            .Where(m => m is not null)
            .Collect()
            .WithCollectionComparer()
            .SelectMany((models, ct) =>
            {
                ct.ThrowIfCancellationRequested();
                var result = models
                    .GroupBy(m => m!.ImplementationType)
                    .Select(g => g.ToEquatableList(ct));

                return result;
            }).Where(m => m.Count > 0);

        var provider = servicesProvider!
            .Combine(settingsProvider!)
            .Select((t, ct) =>
            {
                ct.ThrowIfCancellationRequested();

                IReadOnlyList<ServiceModel> services = t.Left!;
                var settings = t.Right!;

                var builder = new IndentedStringBuilder(
                    IndentedStringBuilderOptions.GeneratedFile with
                    {
                        AmbientCancellationToken = ct,
                        GeneratorName = typeof(Generator).FullName
                    });

                for(var i = 0; i < services.Count; i++)
                {
                    AppendNonAdapterTypes(builder, services[i], settings);
                }

                AppendAdapterType(builder, settings, services);

                var source = builder.ToString();
                var hintName = services[0].ImplementationType.Signature.HintName;

                return (hintName, source);
            });

        IncludedFileSources.RegisterToContext(context);
        context.RegisterSourceOutput(provider, (ctx, t) => ctx.AddSource(t.hintName, t.source));
    }
    static void AppendNonAdapterTypes(IndentedStringBuilder builder, ServiceModel service, SettingsModel settings)
    {
        _ = builder.Operators +
            Append(b =>
            {
                if(service!.Namespace is not [.., { }])
                    return;

                _ = b.Append("namespace ").Append(service!.Namespace).OpenBracesBlock();
            }) +
                Append(b =>
                {
                    if(service!.RequestTypeKind is RequestTypeKind.MatchParameterType)
                        return;

                    _ = b.Operators +
                    Append(service.Visibility ?? settings!.DefaultVisibility) + ' ' +
                    Append(service.RequestTypeKind) + ' ' +
                    service.RequestTypeName + '(' +
                        Append(b =>
                        {
                            for(var i = 0; i < service.Parameters.Count; i++)
                            {
                                if(i > 0)
                                    b.AppendCore(',');

                                var paramModel = service.Parameters[i];
                                b.Append(paramModel.Type).Append(' ').AppendCore(paramModel.PropertyName);
                            }
                        }) + ')' +
                        " : " + settings!.RequestInterfaceTypeName + '<' + service.ResultTypeFullName + '>' + ';' + NewLine;
                }) +
                Append(service!.Visibility ?? settings!.DefaultVisibility) + " partial interface " + service.ServiceInterfaceName +
                OpenBracesBlock() +
                    Append(b =>
                    {
                        b.Append(Constants.ValueTaskTypeName).Append('<').Append(service.ResultTypeFullName).Append("> ")
                        .Append(service.RequestTypeName).Append('(')
                        .Append(b =>
                        {
                            foreach(var (type, name, _) in service.Parameters)
                            {
                                b.Append(type).Append(' ').Append(name).AppendCore(", ");
                            }
                        })
                        .Append(Constants.CtTypeName).AppendCore(" cancellationToken);");
                    }) +
                CloseBlock() +
            CloseAllBlocks() +
            Append(b =>
            {
                if(service.ImplementationType.Namespace is not [.., { }])
                    return;

                _ = b.Append("namespace ").Append(service.ImplementationType.Namespace).OpenBracesBlock();
            }) +
                Append(b =>
                {
                    var signatures = new Stack<TypeSignatureModel>();
                    var implTypeSig = service.ImplementationType.Signature;
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
                    '[' + settings!.ServiceInjectionInfoAttributeTypeName +
                    '(' + NewLine +
                    Indent() +
                        "RequestType = typeof(" + service.RequestTypeFullName + ")," + NewLine +
                        "ResultType = typeof(" + service.ResultTypeFullName + ")," + NewLine +
                        "ServiceType = typeof(" + service.ServiceInterfaceFullName + ")," + NewLine +
                        "ImplementationType = typeof(" + service.ImplementationType.Signature.FullDisplayName + ")," + NewLine +
                        "AdapterType = typeof(" + service.ImplementationType.Signature.Name + "Adapter" + NewLine +
                    Detent() + NewLine +
                    "))]" + NewLine +
                    "partial " + implTypeSig.RecordKeyword + ' ' + implTypeSig.TypeKind + ' ' + implTypeSig.Name +
                    " : " + AppendServiceType(service, settings) +
                    OpenBracesBlock() +
                        "global::System.Threading.Tasks.ValueTask<" + service.ResultTypeFullName + "> " +
                        AppendServiceType(service, settings) + '.' + settings.ExecuteName + '(' +
                        service.RequestTypeFullName + " request, " + Constants.CtTypeName + " cancellationToken) =>" + NewLine +
                        Indent() +
                            Append(b =>
                            {
                                if(!service.ReturnsValueTask)
                                {
                                    b.Append("new ").Append(Constants.ValueTaskTypeName).Append('<').Append(service.ResultTypeFullName).AppendCore(">(");
                                }

                                _ = b.AppendAdapterInvocation(service);

                                if(!service.ReturnsValueTask)
                                {
                                    b.AppendCore(')');
                                }

                                b.AppendCore(';');
                            }) +
                        Detent() +
                    CloseBlock();
                }) +
            CloseAllBlocks();
    }

    static void AppendAdapterType(IndentedStringBuilder builder, SettingsModel settings, IReadOnlyList<ServiceModel> services)
    {
        _ = builder.Operators +
            "file sealed class " + services[0].ImplementationType.Signature.Name + "Adapter(" +
            Append(b =>
            {
                for(var i = 0; i < services.Count; i++)
                {
                    if(i > 0)
                        b.AppendCore(",");

                    b.AppendLine().AppendServiceType(services[i], settings).Append(" service_").AppendCore(i.ToString());
                }
            }) + ") : " +
            Append(b =>
            {
                var addedInterfaces = new HashSet<String>();
                for(var i = 0; i < services.Count; i++)
                {
                    if(!addedInterfaces.Add(services[i].ServiceInterfaceFullName))
                        continue;

                    if(i > 0)
                        b.AppendCore(",");

                    b.AppendLine().AppendCore(services[i].ServiceInterfaceFullName);
                }
            }) +
            OpenBracesBlock() +
                Append(b =>
                {
                    for(var i = 0; i < services.Count; i++)
                    {
                        b.Append(Constants.ValueTaskTypeName).Append('<').Append(services[i].ResultTypeFullName).Append("> ")
                        .Append(services[i].ServiceInterfaceFullName)
                        .Append('.')
                        .Append(services[i].RequestTypeName).Append('(')
                        .Append(b =>
                        {
                            foreach(var (type, name, _) in services[i].Parameters)
                            {
                                b.Append(type).Append(' ').Append(name).AppendCore(", ");
                            }
                        })
                        .Append(Constants.CtTypeName).Append(" cancellationToken) => ").AppendLine()
                        .Indent()
                            .Append("service_").Append(i.ToString()).Append('.').Append(settings!.ExecuteName).Append("(new ").Append(services[i].RequestTypeFullName).AppendLine('(')
                            .Indent()
                                .Append(b =>
                                {
                                    if(services[i].Parameters.Count == 0)
                                        return;

                                    var firstParam = services[i].Parameters[0];
                                    b.Append(firstParam.PropertyName).Append(": ").AppendCore(firstParam.Name);

                                    for(var j = 1; j < services[i].Parameters.Count; j++)
                                    {
                                        var param = services[i].Parameters[j];
                                        b.AppendLine(',').Append(param.PropertyName).Append(": ").AppendCore(param.Name);
                                    }
                                })
                            .Detent()
                            .AppendLine("), cancellationToken);")
                            .AppendLine()
                        .DetentCore();
                    }
                }) +
            CloseAllBlocks();
    }
}
