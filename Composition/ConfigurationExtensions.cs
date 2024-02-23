namespace RhoMicro.ApplicationFramework.Composition;

using Microsoft.Extensions.Configuration;

using System.Globalization;

using RhoMicro.ApplicationFramework.Aspects;

internal static class ConfigurationExtensions
{
    public static String GetRequiredString(this IConfiguration configuration, String key, String section)
    {
        var result = configuration.GetRequiredSection(key).Value ??
            throw new InvalidOperationException($"No value supplied for '{key}' in section '{section}'.");

        return result;
    }
    public static Int32 GetRequiredInt32Value(this IConfiguration configuration, String section)
    {
        var value = configuration.GetRequiredSection(section).Value;

        var result = Int32.TryParse(value, out var r) ?
            r :
            throw new InvalidOperationException($"Unable to parse Int32 '{section}' from config.");

        return result;
    }
    public static Double GetRequiredDoubleValue(this IConfiguration configuration, String section)
    {
        var value = configuration.GetRequiredSection(section).Value;

        var result = Double.TryParse(value, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out var r) ?
            r :
            throw new InvalidOperationException($"Unable to parse Double '{section}' from config.");

        return result;
    }
    public static TimeSpan GetRequiredTimespanValue(this IConfiguration configuration, String section)
    {
        var value = configuration.GetRequiredSection(section).Value;

        var result = TimeSpan.TryParse(value, DateTimeFormatInfo.InvariantInfo, out var r) ?
            r :
            throw new InvalidOperationException($"Unable to parse Timespan '{section}' from config.");

        return result;
    }
    public static CacheOptions<TKey> GetRequiredCacheOptions<TKey>(this IConfiguration configuration)
    {
        var result = configuration.GetRequiredCacheOptions<TKey>(EqualityComparer<TKey>.Default);

        return result;
    }
    public static CacheOptions<TKey> GetRequiredCacheOptions<TKey>(this IConfiguration configuration, IEqualityComparer<TKey> keyComparer)
    {
        var lifespan = configuration.GetRequiredTimespanValue($"CacheOptions:{typeof(TKey).Name}:Lifespan");
        var result = new CacheOptions<TKey>(lifespan, keyComparer);

        return result;
    }
}
