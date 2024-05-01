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

using static RhoMicro.CodeAnalysis.Library.Text.IndentedStringBuilder.Appendables;

/// <summary>
/// Generates render mode proxy and wrapper types for render mode interception.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class RenderModeGenerator : IIncrementalGenerator
{
    private const String _rootNamespaceAttributeSource =
        """
        namespace RhoMicro.ApplicationFramework.Hosting;
        [global::System.AttributeUsage(global::System.AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
        internal sealed class RootNamespaceAttribute(global::System.String rootNamespace) : global::System.Attribute
        {
            public global::System.String RootNamespace { get; } = rootNamespace;
        }
        """;
    private const String _rootNamespaceAttributeMetadataName = "RhoMicro.ApplicationFramework.Hosting.RootNamespaceAttribute";
    private const String _rootNamespaceAttributeHintName = "RhoMicro_ApplicationFramework_Hosting_RootNamespaceAttribute.g.cs";
    private const String _optionalAutoAttributeMetadataName = "RhoMicro.ApplicationFramework.Hosting.OptionalInteractiveAutoRenderModeAttribute";
    private const String _optionalServerAttributeMetadataName = "RhoMicro.ApplicationFramework.Hosting.OptionalInteractiveServerRenderModeAttribute";
    private const String _optionalWasmAttributeMetadataName = "RhoMicro.ApplicationFramework.Hosting.OptionalInteractiveWebAssemblyRenderModeAttribute";
    private const String _optionalNullAttributeMetadataName = "RhoMicro.ApplicationFramework.Hosting.OptionalNullRenderModeAttribute";
    private const String _optionalNoOpAttributeMetadataName = "RhoMicro.ApplicationFramework.Hosting.NoOpRenderModeAttribute";

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
    private const String _noOpRenderModeType = "global::RhoMicro.ApplicationFramework.Hosting.NoOpRenderMode";
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

    private static String GetSource(
        String @namespace,
        String[] usingNamespaces,
        String className,
        String renderModeExpr,
        String[] typeParameters,
        String[] typeConstraints,
        CancellationToken ct)
    {
        var typeParametersString = typeParameters.Length > 0
            ? $"<{String.Join(", ", typeParameters)}>"
            : String.Empty;
        var typeParametersOpenString = typeParameters.Length > 0
            ? $"<{String.Concat(Enumerable.Repeat(',', typeParameters.Length - 1))}>"
            : String.Empty;
        var fullyQualifiedRenderModeExpr = renderModeExpr switch
        {
            "null" => "null",
            "noop" => _noOpRenderModeInstanceExpr,
            _ => $"global::Microsoft.AspNetCore.Components.Web.RenderMode.{renderModeExpr}"
        };

        var resultBuilderOps = new IndentedStringBuilder(IndentedStringBuilderOptions.GeneratedFile with
        {
            GeneratorName = typeof(RenderModeGenerator).FullName,
            AmbientCancellationToken = ct
        }).Operators +
        "namespace " + @namespace + ';' + NewLine +
        AppendUsings(usingNamespaces) +
        AppendComponent(
            className: className,
            typeParametersString: typeParametersString,
            fullyQualifiedRenderModeExpr: fullyQualifiedRenderModeExpr,
            typeConstraints: typeConstraints) +
        AppendAttributes(
            className: className,
            typeParametersOpenString: typeParametersOpenString,
            typeParameters: typeParameters) +
        AppendWrapper(
            className: className,
            typeParametersString: typeParametersString,
            noOpRenderModeType: _noOpRenderModeType,
            typeConstraints: typeConstraints) +
        AppendProxy(
            className: className,
            typeParametersString: typeParametersString,
            typeConstraints: typeConstraints);

        var result = resultBuilderOps.Builder.ToString();

        return result;
    }
    #endregion
}
