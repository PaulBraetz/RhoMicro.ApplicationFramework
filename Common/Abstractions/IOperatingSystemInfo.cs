namespace RhoMicro.ApplicationFramework.Common.Abstractions;
using System;

/// <summary>
/// Provides information on an operating system.
/// </summary>
public interface IOperatingSystemInfo
{
    /// <summary>
    /// Gets the name of the operating system.
    /// </summary>
    public String Name { get; }
    /// <inheritdoc cref="OperatingSystem.IsAndroid"/>
    Boolean IsAndroid();
    /// <inheritdoc cref="OperatingSystem.IsAndroidVersionAtLeast"/>
    Boolean IsAndroidVersionAtLeast(Int32 major, Int32 minor = 0, Int32 build = 0, Int32 revision = 0);
    /// <inheritdoc cref="OperatingSystem.IsBrowser"/>
	Boolean IsBrowser();
    /// <inheritdoc cref="OperatingSystem.IsFreeBSD"/>
	Boolean IsFreeBSD();
    /// <inheritdoc cref="OperatingSystem.IsFreeBSDVersionAtLeast"/>
	Boolean IsFreeBSDVersionAtLeast(Int32 major, Int32 minor = 0, Int32 build = 0, Int32 revision = 0);
    /// <inheritdoc cref="OperatingSystem.IsIOSVersionAtLeast"/>
	Boolean IsIOSVersionAtLeast(Int32 major, Int32 minor = 0, Int32 build = 0);
    /// <inheritdoc cref="OperatingSystem.IsLinux"/>
	Boolean IsLinux();
    /// <inheritdoc cref="OperatingSystem.IsMacCatalyst"/>
	Boolean IsMacCatalyst();
    /// <inheritdoc cref="OperatingSystem.IsMacCatalystVersionAtLeast"/>
	Boolean IsMacCatalystVersionAtLeast(Int32 major, Int32 minor = 0, Int32 build = 0);
    /// <inheritdoc cref="OperatingSystem.IsMacOS"/>
	Boolean IsMacOS();
    /// <inheritdoc cref="OperatingSystem.IsMacOSVersionAtLeast"/>
	Boolean IsMacOSVersionAtLeast(Int32 major, Int32 minor = 0, Int32 build = 0);
    /// <inheritdoc cref="OperatingSystem.IsOSPlatform"/>
	Boolean IsOSPlatform(String platform);
    /// <inheritdoc cref="OperatingSystem.IsOSPlatformVersionAtLeast"/>
	Boolean IsOSPlatformVersionAtLeast(String platform, Int32 major, Int32 minor = 0, Int32 build = 0, Int32 revision = 0);
    /// <inheritdoc cref="OperatingSystem.IsTvOS"/>
	Boolean IsTvOS();
    /// <inheritdoc cref="OperatingSystem.IsTvOSVersionAtLeast"/>
	Boolean IsTvOSVersionAtLeast(Int32 major, Int32 minor = 0, Int32 build = 0);
    /// <inheritdoc cref="OperatingSystem.IsWasi"/>
	Boolean IsWasi();
    /// <inheritdoc cref="OperatingSystem.IsWatchOS"/>
	Boolean IsWatchOS();
    /// <inheritdoc cref="OperatingSystem.IsWatchOSVersionAtLeast"/>
	Boolean IsWatchOSVersionAtLeast(Int32 major, Int32 minor = 0, Int32 build = 0);
    /// <inheritdoc cref="OperatingSystem.IsWindows"/>
	Boolean IsWindows();
    /// <inheritdoc cref="OperatingSystem.IsWindowsVersionAtLeast"/>
	Boolean IsWindowsVersionAtLeast(Int32 major, Int32 minor = 0, Int32 build = 0, Int32 revision = 0);
}
