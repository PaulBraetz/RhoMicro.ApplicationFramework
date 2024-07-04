namespace RhoMicro.ApplicationFramework.Aspects;

using Microsoft.CodeAnalysis;

sealed record TypeModel
{
    public required String Namespace { get; init; }
    public required TypeSignatureModel Signature { get; init; }
    public required String? BaseType { get; init; }
    public static TypeModel Create(ITypeSymbol t, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var signature = TypeSignatureModel.Create(t, ct);
        var @namespace = t.ContainingNamespace?.ToDisplayString(
            SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)) ??
            String.Empty;

        var result = new TypeModel()
        {
            BaseType = t.BaseType?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            Namespace = @namespace,
            Signature = signature
        };

        return result;
    }
}
