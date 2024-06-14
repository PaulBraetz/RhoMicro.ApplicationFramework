namespace RhoMicro.ApplicationFramework.Hosting;
using System;
using System.Text.Json;

sealed class ApiServiceSettingsFactory(ApiServicesSettings settings, JsonSerializerOptions serializerOptions)
{
    private readonly Lazy<Dictionary<Type, Object>> _settingsMap = new(() => settings.Services
            .Select(s => s.BuildGenericSettings(settings.BaseUri, serializerOptions))
            .ToDictionary(s => s.GetType(), s => s));
    public Object Create(Type settingsType)
    {
        if(!_settingsMap.Value.TryGetValue(settingsType, out var result))
        {
            throw new InvalidOperationException("Unable to create generic api service settings. Make sure all required api services are properly configured.");
        }

        return result;
    }
}
