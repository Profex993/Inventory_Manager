using Inventory_Manager.Database.DAO;

namespace Inventory_Manager.Forms
{
    public partial class BuildDeviceForum : Form
    {
        private readonly DeviceDAO dao;
        public BuildDeviceForum()
        {
            InitializeComponent();
            this.dao = new();

            DeviceName.Items.AddRange([.. dao.GetAllNames()]);
        }

        public void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                int id = dao.GetIdByName((string)DeviceName.SelectedItem);

                dao.BuildDevice(id, (int)Quantity.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while building device/s: " + ex.Message);
                MessageBox.Show("Error while building device/s.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Close();
        }
    }
}
