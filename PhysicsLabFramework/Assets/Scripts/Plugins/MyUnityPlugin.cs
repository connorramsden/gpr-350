using System.Runtime.InteropServices;

public class MyUnityPlugin
{
    [DllImport("MyUnityPlugin")]
    public static extern int initFoo(int f_new = 0);

    [DllImport("MyUnityPlugin")]
    public static extern int doFoo(int bar = 0);

    [DllImport("MyUnityPlugin")]
    public static extern int termFoo();
}
