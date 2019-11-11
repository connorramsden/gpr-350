using System.Runtime.InteropServices;

public class MyUnityPlugin_Rust
{
    [DllImport("MyUnityPlugin_Rust")]
    public static extern int double_input(int x);

    [DllImport("MyUnityPlugin_Rust")]
    public static extern int triple_input(int x);
}
