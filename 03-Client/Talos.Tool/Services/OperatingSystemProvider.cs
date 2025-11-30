using Talos.Tool.Interfaces;

namespace Talos.Tool.Services;

public class OperatingSystemProvider : IOperatingSystemProvider
{
    public OsType GetCurrentOS()
    {
        if (OperatingSystem.IsWindows())
            return OsType.Windows;
        if (OperatingSystem.IsLinux())
            return OsType.Linux;
        if (OperatingSystem.IsMacOS())
            return OsType.MacOS;
        
        return OsType.Unknown;
    }

    public string GetCurrentOSString()
    {
        return GetCurrentOS() switch
        {
            OsType.Windows => "windows",
            OsType.Linux => "linux",
            OsType.MacOS => "macos",
            _ => "unknown"
        };
    }
}