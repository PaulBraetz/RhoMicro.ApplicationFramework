namespace RhoMicro.ApplicationFramework.Aspects;

using Microsoft.CodeAnalysis;

readonly record struct ParameterModel(
    String Type,
    String Name,
    String PropertyName)
{
    public static ParameterModel Create(IParameterSymbol symbol) => new(
            Type: symbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            Name: symbol.Name,
            PropertyName: $"{Char.ToUpperInvariant(symbol.Name[0])}{symbol.Name[1..]}");
    public static ParameterModel Create(IPropertySymbol symbol) => new(
            Type: symbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            Name: $"{Char.ToLowerInvariant(symbol.Name[0])}{symbol.Name[1..]}",
            PropertyName: symbol.Name);
}
