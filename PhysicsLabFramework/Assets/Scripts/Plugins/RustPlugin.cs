using System.Runtime.InteropServices;
using Phys;

public static class RustPlugin
{
    [DllImport("unity_ffi.dll")]
    public static extern int check_collisions(byte[] bytes);
}
