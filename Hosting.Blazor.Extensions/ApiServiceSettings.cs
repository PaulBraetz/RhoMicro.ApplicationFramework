namespace RhoMicro.ApplicationFramework.Hosting;
using System;

using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;

sealed partial class ApiServiceSettings
{
    partial void OnRequestTypeNameSet(Type requestType, Type resultType, Type requestDtoType, Type resultDtoType);

    private String _request = String.Empty;
    private static readonly Lazy<Dictionary<String, Type>> _requestTypeMap = new(() =>
    {
        var result = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t =>
                t.FullName is [.., { }] &&
                t.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IApiRequest<,,,>))
                .Count() == 1)
            .ToDictionary(t => t.FullName!, t => t);

        return result;
    });
    private static Type? GetRequestType(String name) =>
        _requestTypeMap.Value.TryGetValue(name, out var type)
        ? type
        : null;

    public required String Request
    {
        get => _request;
        set
        {
            try
            {
                SetRequest(value);
            } catch(Exception ex)
            {
                Error = ex;
            }
        }
    }

    private void SetRequest(String value)
    {
        var requestType = GetRequestType(value)
            ?? throw new InvalidOperationException($"Invalid request name provided. Unable to locate request type for '{value}'.");

        var interfaceTypes = requestType.GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IApiRequest<,,,>))
            .ToArray();

        var (resultType, requestDtoType, resultDtoType) = interfaceTypes switch
        {
        [{ } iasrType] => getNonRequestArgs(iasrType),
        [] => throw new InvalidOperationException($"Unable to locate required generic arguments on request type '{requestType}'."),
            _ => throw new InvalidOperationException($"Unable to locate unique required generic arguments on request type '{requestType}'. Multiple api service request implementations were found: {String.Join(',', interfaceTypes.Select(t => t.FullName))}"),
        };

        OnRequestTypeNameSet(requestType, resultType, requestDtoType, resultDtoType);
        _request = value;

        static (Type, Type, Type) getNonRequestArgs(Type iasrType)
        {
            var args = iasrType.GenericTypeArguments;
            return (args[1], args[2], args[3]);
        }
    }

    public required String Endpoint { get; set; }

    void ThrowIfUninitialized()
    {
        if(_request is not [.., { }])
            throw new InvalidOperationException($"{nameof(Request)} was not initialized correctly (was either null or empty).");
    }

    public Optional<Exception> Error { get; private set; } = Optional.None<Exception>();
    public Boolean IsValid => _request is [.., { }] && Error.IsNone && Uri.IsWellFormedUriString(Endpoint, UriKind.Relative);
}

