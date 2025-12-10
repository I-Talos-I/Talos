namespace Talos.Tool.Interfaces;

public enum OsType
{
    Windows,
    Linux,
    MacOS,
    Unknown
}

public interface IOperatingSystemProvider
{
    OsType GetCurrentOS();
    string GetCurrentOSString();
}