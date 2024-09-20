namespace RhoMicro.ApplicationFramework.Hosting;
using System;
/// <summary>
/// Exception thrown by api clients when deserializing result dtos.
/// </summary>
public sealed class ApiServiceDeserializationException : Exception
{
    internal ApiServiceDeserializationException(Type resultDtoType, String serializedDto)
        : base(GetMessage(resultDtoType)) => SerializedDto = serializedDto;
    internal ApiServiceDeserializationException(Type resultDtoType, Exception innerException)
        : base(GetMessage(resultDtoType), innerException) => SerializedDto = String.Empty;
    private static String GetMessage(Type resultDtoType) => $"Unable to deserialize result dto of type {resultDtoType.FullName}.";
    /// <summary>
    /// Gets the serialized dto, if possible. If another exception caused deserialization to fail, this will return <see cref="String.Empty"/>.
    /// </summary>
    public String SerializedDto { get; }
}
