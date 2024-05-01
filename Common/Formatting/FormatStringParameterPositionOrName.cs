namespace RhoMicro.ApplicationFramework.Common;

using RhoMicro.CodeAnalysis;

/// <summary>
/// Represents either a format string parameter name or position.
/// </summary>
[UnionType<FormatStringPosition>(Alias = "Position")]
[UnionType<FormatStringParameter>(Alias = "Name", Storage = StorageOption.Field)]
[UnionTypeSettings(ToStringSetting = ToStringSetting.Simple)]
public readonly partial struct FormatStringPositionOrParameter;

