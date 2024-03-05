namespace RhoMicro.CodeAnalysis.Presentation.Views.Blazor.RenderModeGenerator.Generators;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
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
    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
#if DEBUG
        //Debugger.Launch();
#endif

        var autoProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
            "RhoMicro.ApplicationFramework.Presentation.Views.Blazor.OptionalInteractiveAutoAttribute",
            IsTargetDeclaration, (context, ct) => GetOutput(context, "InteractiveAuto", ct));
        var ssrProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
            "RhoMicro.ApplicationFramework.Presentation.Views.Blazor.OptionalInteractiveServerAttribute",
            IsTargetDeclaration, (context, ct) => GetOutput(context, "InteractiveServer", ct));
        var csrProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
            "RhoMicro.ApplicationFramework.Presentation.Views.Blazor.OptionalInteractiveWebAssemblyAttribute",
            IsTargetDeclaration, (context, ct) => GetOutput(context, "InteractiveWebAssembly", ct));
        var nullProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
            "RhoMicro.ApplicationFramework.Presentation.Views.Blazor.NullRenderModeAttribute",
            IsTargetDeclaration, (context, ct) => GetOutput(context, "null", ct));

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
            .Select((t, ct) => GetOutput(t.Left.Right, t.Left.Left.path, t.Left.Left.razorSource, t.Right, ct));

        RegisterOutput(context, razorProvider);
        RegisterOutput(context, ssrProvider);
        RegisterOutput(context, csrProvider);
        RegisterOutput(context, autoProvider);
        RegisterOutput(context, nullProvider);
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

    private const String _auto = "OptionalInteractiveAuto";
    private const String _server = "OptionalInteractiveServer";
    private const String _webAssembly = "OptionalInteractiveWebAssembly";
    private const String _nulLRenderMode = "NullRenderMode";
    private static readonly Regex _renderModeAttributePattern = new(@"(?<=@attribute \[)(" + _auto + "|" + _server + "|" + _webAssembly + @")(?=\])", RegexOptions.Compiled);
    private static String GetRenderModeExpr(String source, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var match = _renderModeAttributePattern.Match(source) is { Success: true, Captures: [Capture capture] } ? capture.Value : null;
        var result = match switch
        {
            _auto => "InteractiveAuto",
            _server => "InteractiveServer",
            _webAssembly => "InteractiveWebAssembly",
            _ => "null"
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
    private static void GetNamespaceAndClassName(Compilation compilation, String path, String razorSource, out String @namespace, out String className, CancellationToken ct)
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
        var rootNamespace = assemblyName;
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
    private static String GetHintName(String @namespace, String className, String renderModeExpr) => $"{@namespace.Replace('.', '_')}{( @namespace != String.Empty ? "_" : String.Empty )}{className}_{( renderModeExpr == "null" ? _nulLRenderMode : renderModeExpr )}.g.cs";
    #endregion
    #region GetOutput
    private static (String hintName, String source) GetOutput(Compilation compilation, String path, String razorSource, EquatableList<String> importUsings, CancellationToken ct)
    {
        GetNamespaceAndClassName(compilation, path, razorSource, out var @namespace, out var className, ct);
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
    private static String GetSource(String @namespace, String[] usingNamespaces, String className, String renderModeExpr, String[] typeParameters, String[] typeConstraints, CancellationToken ct)
    {
        var typeParametersString = typeParameters.Length > 0 ?
            $"<{String.Join(", ", typeParameters)}>" :
            String.Empty;
        var typeParametersOpenString = typeParameters.Length > 0 ?
            $"<{String.Concat(Enumerable.Repeat(',', typeParameters.Length - 1))}>" :
            String.Empty;
        var fullyQualifiedRenderModeExpr = renderModeExpr != "null" ?
            $"global::Microsoft.AspNetCore.Components.Web.RenderMode.{renderModeExpr}" :
            renderModeExpr;
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
        .Append("partial class ").Append(className).Append(typeParametersString)
        .AppendJoinLines(StringOrChar.Empty, typeConstraints)
        .OpenBracesBlock()
        .AppendLine("[global::Microsoft.AspNetCore.Components.Parameter]")
        .Append("public global::Microsoft.AspNetCore.Components.IComponentRenderMode? OptionalRenderMode { get; set; } = ").Append(fullyQualifiedRenderModeExpr).AppendLine(';')
        .AppendLine("#nullable disable")
        .AppendLine("[global::RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection.Injected]")
        .AppendLine("public global::RhoMicro.ApplicationFramework.Presentation.Views.Blazor.IComponentRenderModeSettings OptionalRenderModeSettings { get; set; }")
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
        //TODO: generate static parameter list
        .Append("private static readonly global::System.Collections.Generic.List<(global::System.String name, global::System.Func<").Append(className).Append(typeParametersString).AppendLine(", global::System.Object?> getAccessor)> _parameters;")
        .AppendLine("protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)")
        .OpenBracesBlock()
        .Append("builder.OpenComponent(0, typeof(RenderModeProxy").Append(typeParametersOpenString).AppendLine("));")
        .AppendLine("for(var i = 0; i < _parameters.Count; i++)")
        .OpenBracesBlock()
        .AppendJoinLines(StringOrChar.Empty, [
                "var (name, getAccessor) = _parameters[i];",
                "var value = getAccessor.Invoke(this);",
                "builder.AddComponentParameter(i + 1, name, value);"
            ])
        .CloseBlock()
        .AppendLine("if(this.OptionalRenderModeSettings.ApplyRenderMode)")
        .OpenBracesBlock()
        .AppendLine("builder.AddComponentRenderMode(OptionalRenderMode);")
        .CloseBlock()
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
    #endregion
}
