using System.Runtime.InteropServices;
using Avalonia.Controls;

namespace Launch_Connection.Services;

public class System
{
    public static string ToString(bool suffix = false)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            if (suffix) return "dll";
            else return "Windows";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            if (suffix) return "dylib";
            else return "MacOS";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            if (suffix) return "so";
            else return "Liunx";
        }

        return null;
    }
}