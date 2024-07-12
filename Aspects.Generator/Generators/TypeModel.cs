namespace RhoMicro.ApplicationFramework.Aspects;

using Microsoft.CodeAnalysis;

sealed record TypeModel
{
    TypeModel(String fullName) => FullName = fullName;

    public required String Namespace { get; init; }
    public required TypeSignatureModel Signature { get; init; }
    public required String? BaseType { get; init; }
    private String FullName { get; }

    public static TypeModel Create(ITypeSymbol t, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var signature = TypeSignatureModel.Create(t, ct);
        var @namespace = t.ContainingNamespace?.ToDisplayString(
            SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)) ??
            String.Empty;

        var fullName = t.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        var result = new TypeModel(fullName)
        {
            BaseType = t.BaseType?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            Namespace = @namespace,
            Signature = signature
        };

        return result;
    }
    public Boolean Equals(TypeModel? other) => other?.FullName == FullName;
    public override Int32 GetHashCode() => FullName.GetHashCode();
}
