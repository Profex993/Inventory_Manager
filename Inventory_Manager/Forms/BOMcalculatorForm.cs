using Inventory_Manager.Database.DAO;
using Inventory_Manager.Entities;

namespace Inventory_Manager.Forms
{
    public partial class BOMcalculatorForm : Form
    {
        private readonly PartsDAO partDao = new();
        private readonly DeviceDAO deviceDao = new();
        public BOMcalculatorForm()
        {
            InitializeComponent();

            deviceComboBox.Items.AddRange([.. deviceDao.GetAllNames()]);
        }

        public void CalculateButton_Click(object sender, EventArgs e)
        {
            if (deviceComboBox.SelectedItem == null || quantityInput.Value <= 0)
            {
                MessageBox.Show("Input not valid.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            int deviceId = deviceDao.GetIdByName(deviceComboBox.SelectedItem.ToString());

            List<PartRequirement> parts = partDao.CalculateBOMlist(deviceId, (int)quantityInput.Value);

            if (parts.Count == 0)
            {
                MessageBox.Show("All parts needed to buid devices are in inventory.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                dataGrid.DataSource = parts;
            }
        }
    }
}
