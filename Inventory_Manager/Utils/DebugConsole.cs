using System.Runtime.InteropServices;

namespace Inventory_Manager.Utils
{
    internal static class DebugConsole
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AllocConsole();
    }
}