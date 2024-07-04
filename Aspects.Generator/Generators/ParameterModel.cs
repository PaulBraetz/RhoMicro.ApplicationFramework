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
}
