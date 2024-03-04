namespace RhoMicro.CodeAnalysis.Presentation.Views.Blazor.RenderModeGenerator.Generators;

using System.Runtime.CompilerServices;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor;
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
        var autoProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
            typeof(OptionalInteractiveAutoAttribute).FullName,
            IsTargetDeclaration, (context, ct) => GetOutput(context, "InteractiveAuto", ct));
        var ssrProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
            typeof(OptionalInteractiveServerAttribute).FullName,
            IsTargetDeclaration, (context, ct) => GetOutput(context, "InteractiveServer", ct));
        var csrProvider = context.SyntaxProvider.ForAttributeWithMetadataName(
            typeof(OptionalInteractiveWebAssemblyAttribute).FullName,
            IsTargetDeclaration, (context, ct) => GetOutput(context, "InteractiveWebAssembly", ct));

        RegisterOutput(context, autoProvider);
        RegisterOutput(context, ssrProvider);
        RegisterOutput(context, csrProvider);
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
    private static (String hintName, String source) GetOutput(GeneratorAttributeSyntaxContext context, String renderModeExpr, CancellationToken ct)
    {
        var source = GetSource(context, renderModeExpr, ct);
        var hintName = GetHintName(context, renderModeExpr, ct);

        var result = (hintName, source);

        return result;
    }

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
    private static String GetSource(GeneratorAttributeSyntaxContext context, String renderModeExpr, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var source = new IndentedStringBuilder(IndentedStringBuilderOptions.GeneratedFile with
        {
            GeneratorName = typeof(RenderModeGenerator).FullName,
            AmbientCancellationToken = ct
        })
        .Append(
            context.TargetSymbol.ContainingNamespace.ToDisplayString(
                SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)))
        .AppendLine(';')
        .AppendLine("[RenderModeHelperComponentsAttributeImpl]")
        .Append("partial class ").Append(context.TargetSymbol.ToDisplayString(
            SymbolDisplayFormat.MinimallyQualifiedFormat.WithGenericsOptions(SymbolDisplayGenericsOptions.IncludeTypeParameters)))
        .AppendLine(';')
        .AppendLine("file sealed class RenderModeHelperComponentsAttributeImpl : global::RhoMicro.ApplicationFramework.Presentation.Views.Blazor.RenderModeHelperComponentsAttribute")
        .OpenBracesBlock()
        .AppendLine("public override Type FrameType => typeof(RenderModeFrame);")
        .AppendLine("public override Type WrapperType => typeof(RenderModeWrapper);")
        .CloseBlock()
        .AppendLine("[global::RhoMicro.ApplicationFramework.Presentation.Views.Blazor.ExcludeComponentFromContainer]")
        .Append("file sealed class RenderModeFrame : ").AppendLine(context.TargetSymbol.Name)
        .OpenBracesBlock()
        .AppendLine("protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)")
        .AppendLine("builder.OpenComponent<RenderModeWrapper>(0);")
        .Append("builder.AddComponentRenderMode(global::Microsoft.AspNetCore.Components.Web.").Append(renderModeExpr).AppendLine(");")
        .AppendLine("builder.CloseComponent();")
        .OpenBracesBlock()
        .CloseBlock()
        .CloseBlock()
        .AppendLine("[global::RhoMicro.ApplicationFramework.Presentation.Views.Blazor.ExcludeComponentFromContainer]")
        .Append("file sealed class RenderModeWrapper : ").Append(context.TargetSymbol.Name).AppendLine(';')
        .ToString();

        return source;
    }
}
