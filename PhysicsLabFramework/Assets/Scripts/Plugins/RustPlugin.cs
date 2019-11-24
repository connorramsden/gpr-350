using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

public static class RustPlugin
{
    [DllImport("unity_ffi.dll")]
    public static extern int test_bool(string str);
}
