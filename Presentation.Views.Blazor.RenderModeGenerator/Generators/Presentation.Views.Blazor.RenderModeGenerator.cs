namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor.RenderModeGenerator.Generators;

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using RhoMicro.CodeAnalysis.Library;
using RhoMicro.CodeAnalysis.Library.Text;

/// <summary>
/// Generates
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class RenderModeGenerator : IIncrementalGenerator
{
    private const String _rootNamespaceAttributeSource =
        """
        namespace RhoMicro.ApplicationFramework.Presentation.Views.Blazor;
        [global::System.AttributeUsage(global::System.AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
        internal sealed class RootNamespaceAttribute(global::System.String rootNamespace) : global::System.Attribute
        {
            public global::System.String RootNamespace { get; } = rootNamespace;
        }
        """;
    private const String _rootNamespaceAttributeMetadataName = "RhoMicro.ApplicationFramework.Presentation.Views.Blazor.RootNamespaceAttribute";
    private const String _rootNamespaceAttributeHintName = "RhoMicro_ApplicationFramework_Presentation_Views_Blazor_RootNamespaceAttribute.g.cs";
    private const String _optionalAutoAttributeMetadataName = "RhoMicro.ApplicationFramework.Presentation.Views.Blazor.OptionalInteractiveAutoRenderModeAttribute";
    private const String _optionalServerAttributeMetadataName = "RhoMicro.ApplicationFramework.Presentation.Views.Blazor.OptionalInteractiveServerRenderModeAttribute";
    private const String _optionalWasmAttributeMetadataName = "RhoMicro.ApplicationFramework.Presentation.Views.Blazor.OptionalInteractiveWebAssemblyRenderModeAttribute";
    private const String _optionalNullAttributeMetadataName = "RhoMicro.ApplicationFramework.Presentation.Views.Blazor.OptionalNullRenderModeAttribute";
    private const String _optionalNoOpAttributeMetadataName = "RhoMicro.ApplicationFramework.Presentation.Views.Blazor.NoOpRenderModeAttribute";

    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
#if DEBUG
        //Debugger.Launch();
#endif
        var rootNamespaceProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
            _rootNamespaceAttributeMetadataName,
            (node, ct) => node is CompilationUnitSyntax,
            (context, ct) =>
            {
                var result = context.Attributes[0] is
                {
                    ConstructorArguments: [{ Kind: TypedConstantKind.Primitive, Value: String rootNamespace }]
                } ? rootNamespace : null;

                return result;
            }).Collect()
            .WithCollectionComparer()
            .Select((roots, ct) => roots.Length > 0 ? roots.First(r => r is not null) : null);

        var autoProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
            _optionalAutoAttributeMetadataName,
            IsTargetDeclaration, (context, ct) => GetOutput(context, "InteractiveAuto", ct));
        var ssrProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
            _optionalServerAttributeMetadataName,
            IsTargetDeclaration, (context, ct) => GetOutput(context, "InteractiveServer", ct));
        var csrProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
            _optionalWasmAttributeMetadataName,
            IsTargetDeclaration, (context, ct) => GetOutput(context, "InteractiveWebAssembly", ct));
        var nullProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
            _optionalNullAttributeMetadataName,
            IsTargetDeclaration, (context, ct) => GetOutput(context, "null", ct));
        var noOpProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
            _optionalNoOpAttributeMetadataName,
            IsTargetDeclaration, (context, ct) => GetOutput(context, "noop", ct));

        //retrieving razor components seems to only be possible through the actual razor files;
        //I have not found a way yet to access the generated razor classes, as of net8.
        var importUsingsProvider = context.AdditionalTextsProvider
            .Where(text => Path.GetFileName(text.Path) == "_Imports.razor")
            .Select((text, ct) =>
            {
                ct.ThrowIfCancellationRequested();
                var razorSource = text.GetText(ct)?.ToString() ?? String.Empty;
                var usings = GetUsings(razorSource).AsEquatable();

                return usings;
            }).Collect()
            .WithCollectionComparer()
            .Select((usings, ct) => usings.SelectMany(l => l).ToEquatableList(ct));

        var razorProvider = context.AdditionalTextsProvider
            .Where(text => Path.GetExtension(text.Path) == ".razor" && Path.GetFileNameWithoutExtension(text.Path) != "_Imports")
            .Select((text, ct) => (path: text.Path, razorSource: text.GetText(ct)?.ToString() ?? String.Empty))
            .Combine(context.CompilationProvider)
            .Combine(importUsingsProvider)
            .Combine(rootNamespaceProvider)
            .Select((t, ct) => GetOutput(t.Left.Left.Right, t.Left.Left.Left.path, t.Left.Left.Left.razorSource, t.Left.Right, t.Right, ct));

        RegisterOutput(context, razorProvider);
        RegisterOutput(context, ssrProvider);
        RegisterOutput(context, csrProvider);
        RegisterOutput(context, autoProvider);
        RegisterOutput(context, nullProvider);
        context.RegisterPostInitializationOutput(ctx => ctx.AddSource(_rootNamespaceAttributeHintName, _rootNamespaceAttributeSource));
    }
    private static void RegisterOutput(
        IncrementalGeneratorInitializationContext context,
        IncrementalValuesProvider<(String hintName, String source)> provider) =>
        context.RegisterSourceOutput(provider, static (context, output) => context.AddSource(output.hintName, output.source));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Boolean IsTargetDeclaration(SyntaxNode node, CancellationToken ct) =>
        node is ClassDeclarationSyntax
        {
            Modifiers: [.., { RawKind: (Int32)SyntaxKind.PartialKeyword }]
        };
    #region Razor Examination

    private static readonly Regex _typeParamPattern = new(@"(?<=@typeparam )(?<name>[a-zA-Z][a-zA-Z0-9]*)(?<constraint>[^\r\n]*)?", RegexOptions.Compiled);
    private static void GetTypeParametersAndConstraints(String razorSource, out String[] typeParameters, out String[] typeParameterConstraints, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        if(_typeParamPattern.Matches(razorSource) is not { Count: > 0 } matchCollection)
        {
            typeParameters = [];
            typeParameterConstraints = [];
            return;
        }

        var matches = matchCollection.OfType<Match>();
        typeParameters = matches.Select(m => m.Groups["name"].Success ? m.Groups["name"].Value : String.Empty)
            .Where(name => name != String.Empty)
            .ToArray();
        typeParameterConstraints = GetConstraints(matches);
    }

    private static String[] GetConstraints(IEnumerable<Match> matches)
    {
        var result = matches.Select(m => m.Groups["constraint"].Success ? m.Groups["constraint"].Value : String.Empty)
                    .Where(constraint => constraint != String.Empty)
                    .ToArray();

        return result;
    }

    private static readonly Regex _usingsPattern = new(@"(?<=@using )((([a-zA-Z_]+[a-zA-Z0-9_]*)\s*=\s*)|(static\s*))?(([a-zA-Z_]+[a-zA-Z0-9_]*)(\.[a-zA-Z_]+[a-zA-Z0-9_]*)*)", RegexOptions.Compiled);
    private static String[] GetUsings(String razorSource, IEnumerable<String>? additionalUsings = null)
    {
        additionalUsings ??= [];
        var matches = _usingsPattern.Matches(razorSource)
                    .OfType<Match>()
                    .Where(m => m.Success)
                    .Select(m => m.Value);

        var result = matches.Concat(additionalUsings).Distinct().ToArray();

        return result;
    }

    private const String _autoRenderMode = "OptionalInteractiveAutoRenderMode";
    private const String _serverRenderMode = "OptionalInteractiveServerRenderMode";
    private const String _webAssemblyRenderMode = "OptionalInteractiveWebAssemblyRenderMode";
    private const String _nullRenderMode = "OptionalNullRenderMode";
    private const String _noOpRenderModeType = "global::RhoMicro.ApplicationFramework.Presentation.Views.Blazor.NoOpRenderMode";
    private const String _noOpRenderModeInstanceExpr = _noOpRenderModeType + ".Instance";
    private static readonly Regex _renderModeAttributePattern = new(@"(?<=@attribute \[)(" + _autoRenderMode + "|" + _serverRenderMode + "|" + _webAssemblyRenderMode + "|" + _nullRenderMode + @")(?=\])", RegexOptions.Compiled);
    private static String GetRenderModeExpr(String source, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var match = _renderModeAttributePattern.Match(source) is { Success: true, Captures: [Capture capture] } ? capture.Value : null;
        var result = match switch
        {
            _autoRenderMode => "InteractiveAuto",
            _serverRenderMode => "InteractiveServer",
            _webAssemblyRenderMode => "InteractiveWebAssembly",
            _nullRenderMode => "null",
            _ => "noop"
        };

        return result;
    }
    private static readonly Regex _namespaceDirectivePattern = new(@"(?<=@namespace )[a-zA-Z][a-zA-Z0-9\.]*", RegexOptions.Compiled);
    private static Boolean TryGetNamespaceFromDirective(String source, CancellationToken ct, [NotNullWhen(true)] out String? @namespace)
    {
        ct.ThrowIfCancellationRequested();

        var match = _namespaceDirectivePattern.Match(source);
        if(match is { Success: true, Captures: [Capture capture] })
        {
            @namespace = capture.Value;
            return true;
        }

        @namespace = null;
        return false;
    }
    private static void GetNamespaceAndClassName(Compilation compilation, String path, String razorSource, String? rootNamespace, out String @namespace, out String className, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        /*
        The namespace of a component authored with Razor is based on the following (in priority order):
        
        The @namespace directive in the Razor file's markup (for example, @namespace BlazorSample.CustomNamespace).
        
        The project's RootNamespace in the project file (for example, <RootNamespace>BlazorSample</RootNamespace>).
        
        The project namespace and the path from the project root to the component. For example, the framework resolves {PROJECT NAMESPACE}/Components/Pages/Home.razor with a project namespace of BlazorSample to the namespace BlazorSample.Components.Pages for the Home component. {PROJECT NAMESPACE} is the project namespace. Components follow C# name binding rules. For the Home component in this example, the components in scope are all of the components:
            In the same folder, Components/Pages.
            The components in the project's root that don't explicitly specify a different namespace.
        
        The following are not supported:
        
        The global:: qualification.
        Partially-qualified names. For example, you can't add @using BlazorSample.Components to a component and then reference the NavMenu component in the app's Components/Layout folder (Components/Layout/NavMenu.razor) with <Layout.NavMenu></Layout.NavMenu>.

        */
        if(TryGetNamespaceFromDirective(razorSource, ct, out var directiveNamespace))
        {
            @namespace = directiveNamespace;
            className = Path.GetFileNameWithoutExtension(path);
            return;
        }

        var assemblyName = compilation.AssemblyName != null ?
            String.Join(".", compilation.AssemblyName.Split(['.'], StringSplitOptions.RemoveEmptyEntries).TakeWhile(s => !Char.IsDigit(s[0]))) :
            String.Empty;
        //[foo, bar, foobar, razor] <- skip razor; [^2] is className
        var namespaceParts = path.Replace(Path.DirectorySeparatorChar, '.')
            .Split([assemblyName], StringSplitOptions.None)[^1]
            .Split(['.'], StringSplitOptions.RemoveEmptyEntries);
        //TODO: obtain root namespace (set in MsBuild) as first try, assembly name as fallback
        //For now, explicit root namespace from MsBuild is unsupported
        rootNamespace ??= assemblyName;
        className = namespaceParts[^2];
        if(namespaceParts.Length == 2)
        {
            @namespace = rootNamespace;
            return;
        }

        @namespace = $"{rootNamespace}{( rootNamespace != String.Empty ? "." : String.Empty )}{String.Join(".", namespaceParts.Take(namespaceParts.Length - 2))}";
    }
    #endregion
    #region GetHintName
    private static String GetHintName(GeneratorAttributeSyntaxContext context, String renderModeExpr, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var typeHint = context.TargetSymbol.ToDisplayString(
            SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted))
            .Replace('.', '_')
            .Replace("<", "_of_")
            .Replace(",", "_and_")
            .Replace(">", String.Empty)
            .Replace(" ", String.Empty);
        var result = $"{typeHint}_{renderModeExpr}.g.cs";

        return result;
    }
    private static String GetHintName(String @namespace, String className, String renderModeExpr) => $"{@namespace.Replace('.', '_')}{( @namespace != String.Empty ? "_" : String.Empty )}{className}_{( renderModeExpr == "null" ? _nullRenderMode : renderModeExpr )}.g.cs";
    #endregion
    #region GetOutput
    private static (String hintName, String source) GetOutput(Compilation compilation, String path, String razorSource, EquatableList<String> importUsings, String? rootNamespace, CancellationToken ct)
    {
        GetNamespaceAndClassName(compilation, path, razorSource, rootNamespace, out var @namespace, out var className, ct);
        GetTypeParametersAndConstraints(razorSource, out var typeParameters, out var typeConstraints, ct);
        var renderModeExpr = GetRenderModeExpr(razorSource, ct);
        var usings = GetUsings(razorSource, importUsings);

        var hintName = GetHintName(@namespace, className, renderModeExpr);
        var source = GetSource(@namespace, usings, className, renderModeExpr, typeParameters, typeConstraints, ct);

        var result = (hintName, source);

        return result;
    }
    private static (String hintName, String source) GetOutput(GeneratorAttributeSyntaxContext context, String renderModeExpr, CancellationToken ct)
    {
        var source = GetSource(context, renderModeExpr, ct);
        var hintName = GetHintName(context, renderModeExpr, ct);

        var result = (hintName, source);

        return result;
    }
    #endregion
    #region GetSource
    private static String GetSource(GeneratorAttributeSyntaxContext context, String renderModeExpr, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var targetSymbol = (INamedTypeSymbol)context.TargetSymbol;
        var @namespace = targetSymbol.ContainingNamespace.ToDisplayString(
            SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted));
        var className = context.TargetSymbol.Name;
        var typeParameters = targetSymbol.TypeParameters.Select(t => t.Name).ToArray();
        var typeConstraints = targetSymbol.TypeParameters.Select(p =>
        {
            var constraints = new List<String>();

            //primary_constraint
            if(p.HasReferenceTypeConstraint)
                constraints.Add("class");
            else if(p.HasValueTypeConstraint)
                constraints.Add("struct");
            else if(p.HasNotNullConstraint)
                constraints.Add("notnull");
            else if(p.HasUnmanagedTypeConstraint)
                constraints.Add("unmanaged");

            //secondary_constraints
            for(var i = 0; i < p.ConstraintTypes.Length; i++)
            {
                var constraint = p.ConstraintTypes[i].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                constraints.Add(constraint);
            }

            //constructor_constraint
            if(p.HasConstructorConstraint)
                constraints.Add("new()");

            var result = constraints.Count == 0 ?
                String.Empty :
                $"where {p.Name} : {String.Join(", ", constraints)}";

            return result;
        }).ToArray();

        var result = GetSource(@namespace, [], className, renderModeExpr, typeParameters, typeConstraints, ct);

        return result;
    }
    private static String GetSource(String @namespace, String[] usingNamespaces, String className, String renderModeExpr, String[] typeParameters, String[] typeConstraints, CancellationToken ct)
    {
        var typeParametersString = typeParameters.Length > 0 ?
            $"<{String.Join(", ", typeParameters)}>" :
            String.Empty;
        var typeParametersOpenString = typeParameters.Length > 0 ?
            $"<{String.Concat(Enumerable.Repeat(',', typeParameters.Length - 1))}>" :
            String.Empty;
        var fullyQualifiedRenderModeExpr = renderModeExpr switch
        {
            "null" => renderModeExpr,
            "noop" => _noOpRenderModeInstanceExpr,
            _ => $"global::Microsoft.AspNetCore.Components.Web.RenderMode.{renderModeExpr}"
        };

        var result = new IndentedStringBuilder(IndentedStringBuilderOptions.GeneratedFile with
        {
            GeneratorName = typeof(RenderModeGenerator).FullName,
            AmbientCancellationToken = ct
        })
        .Append("namespace ").Append(@namespace).AppendLine(';')
        .Append(c =>
        {
            for(var i = 0; i < usingNamespaces.Length; i++)
            {
                c.Append("using ").Append(usingNamespaces[i]).Append(';').AppendLineCore();
            }
        })
        .AppendLine("[RenderModeHelperComponentsAttributeImpl]")
        .Append("partial class ").Append(className).Append(typeParametersString).Append(" : global::RhoMicro.ApplicationFramework.Presentation.Views.Blazor.IOptionalRenderModeComponent")
        .AppendJoinLines(StringOrChar.Empty, typeConstraints)
        .OpenBracesBlock()
        .AppendLine("[global::Microsoft.AspNetCore.Components.CascadingParameter(Name = nameof(ParentOptionalRenderMode))]")
        .Append("public global::Microsoft.AspNetCore.Components.IComponentRenderMode? ParentOptionalRenderMode { get; set; } = ").Append(_noOpRenderModeInstanceExpr).AppendLine(';')
        .AppendLine("[global::Microsoft.AspNetCore.Components.Parameter]")
        .Append("public global::Microsoft.AspNetCore.Components.IComponentRenderMode? OptionalRenderMode { get; set; } = ").Append(fullyQualifiedRenderModeExpr).AppendLine(';')
        .AppendLine("#nullable disable")
        .AppendLine("[global::RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection.Injected]")
        .AppendLine("public global::RhoMicro.ApplicationFramework.Presentation.Views.Blazor.IRenderModeInterceptor RenderModeInterceptor { get; set; }")
        .AppendLine("#nullable restore")
        .CloseBlock()
        .AppendLine("file sealed class RenderModeHelperComponentsAttributeImpl : global::RhoMicro.ApplicationFramework.Presentation.Views.Blazor.RenderModeHelperComponentsAttribute")
        .OpenBracesBlock()
        .Append("public override global::System.Type WrapperType => typeof(RenderModeWrapper").Append(typeParametersOpenString).AppendLine(");")
        .Append("public override global::System.Type ProxyType => typeof(RenderModeProxy").Append(typeParametersOpenString).AppendLine(");")
        .CloseBlock()
        .AppendLine("file sealed class RenderModeProxyAttributeImpl : global::RhoMicro.ApplicationFramework.Presentation.Views.Blazor.RenderModeProxyAttribute")
        .OpenBracesBlock()
        .Append("public override global::System.Type ComponentType => typeof(").Append(className).Append(typeParametersOpenString).AppendLine(");")
        .CloseBlock()
        .AppendLine("file sealed class RenderModeWrapperAttributeImpl : global::RhoMicro.ApplicationFramework.Presentation.Views.Blazor.RenderModeWrapperAttribute")
        .OpenBracesBlock()
        .Append("public override global::System.Type ComponentType => typeof(").Append(className).Append(typeParametersOpenString).AppendLine(");")
        .CloseBlock()
        .AppendLine("[global::RhoMicro.ApplicationFramework.Presentation.Views.Blazor.ExcludeComponentFromContainer]")
        .AppendLine("[RenderModeProxyAttributeImpl]")
        .Append("file sealed class RenderModeWrapper").Append(typeParametersString).Append(" : ").Append(className).AppendLine(typeParametersString)
        .AppendJoinLines(StringOrChar.Empty, typeConstraints)
        .OpenBracesBlock()
        .AppendLine("static RenderModeWrapper()")
        .OpenBracesBlock()
        .AppendLine("_parameters = [];")
        .Append("var propertyInfos = typeof(").Append(className).Append(typeParametersString).Append(").GetProperties(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public);")
        .AppendLine("foreach(var propertyInfo in propertyInfos)")
        .OpenBracesBlock()
        .AppendJoinLines(StringOrChar.Empty, [
            "if(global::System.Reflection.CustomAttributeExtensions.GetCustomAttribute<global::Microsoft.AspNetCore.Components.ParameterAttribute>(propertyInfo) == null) continue;",
            "var accessorInfo = propertyInfo.GetMethod;",
            "if(accessorInfo == null) continue;"])
        .Append("var parameter = global::System.Linq.Expressions.Expression.Parameter(typeof(").Append(className).Append(typeParametersOpenString).AppendLine("));")
        .AppendLine("var callExpr = global::System.Linq.Expressions.Expression.Call(parameter, accessorInfo);")
        .Append("var getAccessor = (global::System.Func<").Append(className).Append(typeParametersString).AppendLine(", global::System.Object?>)global::System.Linq.Expressions.Expression.Lambda(callExpr, parameter).Compile();")
        .AppendLine("var name = propertyInfo.Name;")
        .AppendLine("_parameters.Add((name, getAccessor));")
        .CloseBlock()
        .CloseBlock()
        //TODO: generate static parameter list instead of reflection
        .Append("private static readonly global::System.Collections.Generic.List<(global::System.String name, global::System.Func<").Append(className).Append(typeParametersString).AppendLine(", global::System.Object?> getAccessor)> _parameters;")
        .AppendLine("protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)")
        .OpenBracesBlock()
        .Comment.OpenSingleLineBlock()
        .AppendLine("As per the documentation (https://learn.microsoft.com/en-us/aspnet/core/blazor/components/render-modes?view=aspnetcore-8.0#render-mode-propagation),")
        .AppendLine("a child component may not switch to a different interactive mode than the parent.")
        .CloseBlock()
        //if parent is neither null nor noop: explicit interactivity mode has been set : child must be parent
        .Append("var actualRenderMode = ").Append("this.ParentOptionalRenderMode is not null and not ").AppendLine(_noOpRenderModeType)
        .AppendLine("? this.ParentOptionalRenderMode")
        .AppendLine(": this.OptionalRenderMode;")
        .AppendLine("if(!this.RenderModeInterceptor.ApplyRenderMode(renderMode: actualRenderMode, component: this))")
        .OpenBracesBlock()
        .AppendLine("base.BuildRenderTree(builder);")
        .AppendLine("return;")
        .CloseBlock()
#if DEBUG
        //.AppendLine("global::System.Diagnostics.Debugger.Break();")
#endif
        .AppendLine("if(EqualityComparer<global::Microsoft.AspNetCore.Components.IComponentRenderMode?>.Default.Equals(actualRenderMode, this.ParentOptionalRenderMode))")
        .OpenBracesBlock()
        .Append("builder.OpenComponent(5, typeof(RenderModeProxy").Append(typeParametersOpenString).AppendLine("));")
        .AppendLine("for(var i = 0; i < _parameters.Count; i++)")
        .OpenBracesBlock()
            .AppendJoinLines(StringOrChar.Empty, [
                    "var (name, getAccessor) = _parameters[i];",
                    "var value = name == nameof(OptionalRenderMode) ? actualRenderMode : getAccessor.Invoke(this);",
                    "builder.AddComponentParameter(i + 6, name, value);"
                ])
        .CloseBlock()
        .AppendLine("builder.AddComponentRenderMode(actualRenderMode);")
        .AppendLine("builder.CloseComponent();")
        .AppendLine("return;")
        .CloseBlock()
        .AppendJoinLines(StringOrChar.Empty, [
            "builder.OpenComponent<global::Microsoft.AspNetCore.Components.CascadingValue<global::Microsoft.AspNetCore.Components.IComponentRenderMode?>>(0);",
            "builder.AddComponentParameter(1, \"Value\", actualRenderMode);",
            "builder.AddComponentParameter(2, \"Name\", nameof(ParentOptionalRenderMode));",
            "builder.AddComponentParameter(3, \"IsFixed\", false);"
            ])
        .AppendLine()
        .AppendLine("builder.AddAttribute(4, \"ChildContent\", (global::Microsoft.AspNetCore.Components.RenderFragment)(builder=>")
        .OpenBracesBlock()
        .Append("builder.OpenComponent(5, typeof(RenderModeProxy").Append(typeParametersOpenString).AppendLine("));")
        .AppendLine("for(var i = 0; i < _parameters.Count; i++)")
        .OpenBracesBlock()
            .AppendJoinLines(StringOrChar.Empty, [
                    "var (name, getAccessor) = _parameters[i];",
                    "var value = getAccessor.Invoke(this);",
                    "builder.AddComponentParameter(i + 6, name, value);"
                ])
        .CloseBlock()
        .AppendLine("builder.AddComponentRenderMode(actualRenderMode);")
        .AppendLine("builder.CloseComponent();")
        .CloseBlock()
        .AppendLine("));")
        .AppendLine("builder.CloseComponent();")
        .CloseBlock()
        .CloseBlock()
        .AppendLine("[global::RhoMicro.ApplicationFramework.Presentation.Views.Blazor.ExcludeComponentFromContainer]")
        .AppendLine("[RenderModeWrapperAttributeImpl]")
        .Append("file sealed class RenderModeProxy").Append(typeParametersString).Append(" : ").Append(className).AppendLine(typeParametersString)
        .AppendJoinLines(StringOrChar.Empty, typeConstraints)
        .AppendLine(';')
        .ToString();

        return result;
    }
    #endregion
}
