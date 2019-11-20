using System.Runtime.InteropServices;

public static class RustPlugin
{
    [DllImport("unity_ffi.dll")]
    public static extern void deserialize_hull();
}
