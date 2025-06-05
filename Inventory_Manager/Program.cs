using Inventory_Manager.Forms;
using Inventory_Manager.Utils;

namespace Inventory_Manager
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //check for -d argument which opens console
            if (args.Contains("-d"))
            {
                DebugConsole.AllocConsole();
            }

            ApplicationConfiguration.Initialize();

            //show database connection forum and continue based on output
            using var connectionForm = new ConnectionForm();
            if (connectionForm.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new MainForum());
            }
            else
            {
                MessageBox.Show("Invalid connection credentials!", "Failed to connect", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
    }
}