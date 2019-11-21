using System.Runtime.InteropServices;
using System.Text;

public static class RustPlugin
{
    [DllImport("unity_ffi.dll")]
    public static extern int test_bool(StringBuilder str);
}
