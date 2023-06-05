// Copyright 2023. Sylvain Guillet (sylvain.guillet@tutamail.com)

using System.Runtime.InteropServices;

namespace IO.Astrodynamics.DTO;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct BodyVisibilityFromSiteConstraint
{
    public int TargetBodyId;
    public string Aberration;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
    public Window[] Windows;

    public BodyVisibilityFromSiteConstraint(int targetBodyId, string aberration)
    {
        Windows = new Window[1000];
        TargetBodyId = targetBodyId;
        Aberration = aberration;
    }
}