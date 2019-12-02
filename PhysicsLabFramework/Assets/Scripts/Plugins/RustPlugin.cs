using System.Runtime.InteropServices;
using NS_Collision_3D;

public static class RustPlugin
{
    [DllImport("unity_ffi.dll")]
    public static extern int testEnum(ref CH3D.SphereHull obj);
}