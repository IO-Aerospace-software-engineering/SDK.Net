using System.Runtime.InteropServices;

namespace IO.SDK.Net.DTO;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct ByDay
{
    public double TwilightDefinition;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
    public Window[] Windows;
}