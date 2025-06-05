using System.Runtime.InteropServices;

namespace Inventory_Manager.Utils
{
    //class for console debug console
    internal static class DebugConsole
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AllocConsole();
    }
}