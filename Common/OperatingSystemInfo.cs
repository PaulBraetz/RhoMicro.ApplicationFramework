namespace RhoMicro.ApplicationFramework.Common;
using System;

using RhoMicro.ApplicationFramework.Common.Abstractions;

/// <summary>
/// Default implementation of <see cref="IOperatingSystemInfo"/>.
/// </summary>
public sealed class OperatingSystemInfo : IOperatingSystemInfo
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public OperatingSystemInfo()
    {
        Name =
            IsWindows() ? "Windows" :
            IsLinux() ? "Linux" :
            IsMacOS() ? "MacOS" :
            IsBrowser() ? "Browser" :
            IsWasi() ? "Wasi" :
            IsAndroid() ? "Android" :
            IsFreeBSD() ? "FreeBSD" :
            IsMacCatalyst() ? "MacCatalyst" :
            IsTvOS() ? "TvOS" :
            IsWatchOS() ? "WatchOS" :
            "Unknown";
    }

    /// <inheritdoc />
    public String Name { get; }
    /// <inheritdoc />
    public Boolean IsAndroid() => OperatingSystem.IsAndroid();
    /// <inheritdoc />
    public Boolean IsBrowser() => OperatingSystem.IsBrowser();
    /// <inheritdoc />
    public Boolean IsFreeBSD() => OperatingSystem.IsFreeBSD();
    /// <inheritdoc />
    public Boolean IsLinux() => OperatingSystem.IsLinux();
    /// <inheritdoc />
    public Boolean IsMacCatalyst() => OperatingSystem.IsMacCatalyst();
    /// <inheritdoc />
    public Boolean IsMacOS() => OperatingSystem.IsMacOS();
    /// <inheritdoc />
    public Boolean IsTvOS() => OperatingSystem.IsTvOS();
    /// <inheritdoc />
    public Boolean IsWasi() => OperatingSystem.IsWasi();
    /// <inheritdoc />
    public Boolean IsWatchOS() => OperatingSystem.IsWatchOS();
    /// <inheritdoc />
    public Boolean IsWindows() => OperatingSystem.IsWindows();
    /// <inheritdoc />
    public Boolean IsAndroidVersionAtLeast(Int32 major, Int32 minor = 0, Int32 build = 0, Int32 revision = 0) => OperatingSystem.IsAndroidVersionAtLeast(major, minor, build, revision);
    /// <inheritdoc />
    public Boolean IsFreeBSDVersionAtLeast(Int32 major, Int32 minor = 0, Int32 build = 0, Int32 revision = 0) => OperatingSystem.IsFreeBSDVersionAtLeast(major, minor, build, revision);
    /// <inheritdoc />
    public Boolean IsIOSVersionAtLeast(Int32 major, Int32 minor = 0, Int32 build = 0) => OperatingSystem.IsIOSVersionAtLeast(major, minor, build);
    /// <inheritdoc />
    public Boolean IsMacCatalystVersionAtLeast(Int32 major, Int32 minor = 0, Int32 build = 0) => OperatingSystem.IsMacCatalystVersionAtLeast(major, minor, build);
    /// <inheritdoc />
    public Boolean IsMacOSVersionAtLeast(Int32 major, Int32 minor = 0, Int32 build = 0) => OperatingSystem.IsMacOSVersionAtLeast(major, minor, build);
    /// <inheritdoc />
    public Boolean IsOSPlatform(String platform) => OperatingSystem.IsOSPlatform(platform);
    /// <inheritdoc />
    public Boolean IsOSPlatformVersionAtLeast(String platform, Int32 major, Int32 minor = 0, Int32 build = 0, Int32 revision = 0) => OperatingSystem.IsOSPlatformVersionAtLeast(platform, major, minor, build, revision);
    /// <inheritdoc />
    public Boolean IsTvOSVersionAtLeast(Int32 major, Int32 minor = 0, Int32 build = 0) => OperatingSystem.IsTvOSVersionAtLeast(major, minor, build);
    /// <inheritdoc />
    public Boolean IsWatchOSVersionAtLeast(Int32 major, Int32 minor = 0, Int32 build = 0) => OperatingSystem.IsWatchOSVersionAtLeast(major, minor, build);
    /// <inheritdoc />
    public Boolean IsWindowsVersionAtLeast(Int32 major, Int32 minor = 0, Int32 build = 0, Int32 revision = 0) => OperatingSystem.IsWindowsVersionAtLeast(major, minor, build, revision);
}