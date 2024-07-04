namespace RhoMicro.ApplicationFramework.Aspects;
using Microsoft.CodeAnalysis;

internal static class SymbolDisplayFormats
{
    public static readonly SymbolDisplayFormat NonGenericFullyQualifiedFormat = SymbolDisplayFormat.FullyQualifiedFormat
        .WithGenericsOptions(SymbolDisplayGenericsOptions.None);
}