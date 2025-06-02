using Inventory_Manager.Forms;
using Inventory_Manager.Utils;

namespace Inventory_Manager
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //open console
            if (args.Contains("-d"))
            {
                DebugConsole.AllocConsole();
            }
            ApplicationConfiguration.Initialize();

            using var connectionForm = new ConnectionForm();
            if (connectionForm.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new MainForum());
            }
            else
            {
                Application.Exit();
            }
        }
    }
}