using Inventory_Manager.Database;
using Inventory_Manager.Forms;
using Inventory_Manager.Utils;

namespace Inventory_Manager
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            //open console
            DebugConsole.AllocConsole();

            Console.WriteLine(DatabaseConnection.Instance);

            ApplicationConfiguration.Initialize();

            using (var connectionForm = new ConnectionForm())
            {
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
}