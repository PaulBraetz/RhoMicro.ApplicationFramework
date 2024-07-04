namespace RhoMicro.ApplicationFramework.Aspects;

/// <summary>
/// Provides possible type kinds for generated request types.
/// </summary>
#if GENERATOR
[RhoMicro.CodeAnalysis.IncludeFile]
#endif
public enum RequestTypeKind
{
    /// <summary>
    /// The generated request type will be a <see langword="readonly"/> <see langword="record"/> <see langword="struct"/>.
    /// </summary>
    RecordStruct,
    /// <summary>
    /// The generated request type will be a <see langword="record"/> <see langword="class"/>.
    /// </summary>
    RecordClass,
    /// <summary>
    /// The generated request type will be a <see langword="sealed"/> <see langword="record"/> <see langword="class"/>.
    /// </summary>
    SealedRecordClass,
    /// <summary>
    /// The generated request type will be a <see langword="abstract"/> <see langword="record"/> <see langword="class"/>.
    /// </summary>
    AbstractRecordClass,
    /// <summary>
    /// A partial declaration for the first parameters type will be generated as the request type.
    /// </summary>
    MatchParameterType
}
