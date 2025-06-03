using Inventory_Manager.Database.DAO;
using Inventory_Manager.Entities;

namespace Inventory_Manager.Forms
{
    public partial class LogBrokenPartsForm : Form
    {
        private readonly PartsDAO dao = new();
        public LogBrokenPartsForm()
        {
            InitializeComponent();
            CreateForm();
        }

        private void CreateForm()
        {
            var partColumn = new DataGridViewComboBoxColumn
            {
                HeaderText = "Part Name",
                Name = "PartName",
                DataSource = new PartsDAO().GetAllNames()
            };

            var amount = new DataGridViewTextBoxColumn
            {
                HeaderText = "Required Amount",
                Name = "RequiredAmount"
            };

            dataGrid.Columns.Add(partColumn);
            dataGrid.Columns.Add(amount);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                List<Part> parts = [];

                foreach (DataGridViewRow row in dataGrid.Rows)
                {
                    if (row.IsNewRow) continue;

                    string partName = row.Cells["PartName"].Value?.ToString();
                    string qunatityText = row.Cells["RequiredAmount"].Value?.ToString();

                    if (string.IsNullOrWhiteSpace(partName) || string.IsNullOrWhiteSpace(qunatityText))
                        continue;

                    int partId = new PartsDAO().GetIdByName(partName);
                    int requiredQuantity = Math.Abs(int.Parse(qunatityText));

                    parts.Add(new Part
                    {
                        Id = partId,
                        Quantity = requiredQuantity
                    });
                }

                if (parts.Count == 0)
                {
                    MessageBox.Show("No valid parts to log.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (Part part in parts)
                {
                    dao.RemoveQuantity(part.Id, part.Quantity);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("Failed to log broken parts.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Close();
        }
    }
}
