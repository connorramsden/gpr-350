using System.Runtime.InteropServices;

public static class RustPlugin
{
    [DllImport("unity_ffi.dll")]
    public static extern int test_bool();
}
