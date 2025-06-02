using Inventory_Manager.Database.DAO;
using Inventory_Manager.Entities;

namespace Inventory_Manager.Forms
{
    public partial class AddBomForm : Form
    {
        private readonly BomDAO dao;
        public AddBomForm(IDAO dao)
        {
            this.dao = dao as BomDAO;
            InitializeComponent();
            GenerateBomForm();
        }

        private void GenerateBomForm()
        {
            deviceComboBox.Items.AddRange([.. new DeviceDAO().GetAllNames()]);

            var partColumn = new DataGridViewComboBoxColumn
            {
                HeaderText = "Part Name",
                Name = "PartName",
                DataSource = new PartsDAO().GetAllNames()
            };

            var requiredAmountColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Required Amount",
                Name = "RequiredAmount"
            };

            dataGrid.Columns.Add(partColumn);
            dataGrid.Columns.Add(requiredAmountColumn);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedDevice = deviceComboBox.Text;
                if (string.IsNullOrWhiteSpace(selectedDevice))
                {
                    MessageBox.Show("Please select a device.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int deviceId = new DeviceDAO().GetIdByName(selectedDevice);

                var bomList = new List<Bom>();

                foreach (DataGridViewRow row in dataGrid.Rows)
                {
                    if (row.IsNewRow) continue;

                    string partName = row.Cells["PartName"].Value?.ToString();
                    string requiredAmountText = row.Cells["RequiredAmount"].Value?.ToString();

                    if (string.IsNullOrWhiteSpace(partName) || string.IsNullOrWhiteSpace(requiredAmountText))
                        continue;

                    int partId = new PartsDAO().GetIdByName(partName);
                    int requiredAmount = int.Parse(requiredAmountText);

                    bomList.Add(new Bom
                    {
                        DeviceId = deviceId,
                        PartId = partId,
                        RequiredAmount = requiredAmount
                    });
                }

                if (bomList.Count == 0)
                {
                    MessageBox.Show("No valid BOM rows to save.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dao.AddAll(bomList);

                Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("Failed to save BOM entries.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
