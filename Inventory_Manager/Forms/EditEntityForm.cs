using Inventory_Manager.Database.DAO;
using Inventory_Manager.Entities;
using System.Reflection;

namespace Inventory_Manager.Forms
{
    public partial class EditEntityForm : Form
    {
        private readonly Entity entity;
        private readonly Type entityType;
        private readonly IDAO dao;

        public EditEntityForm(Entity entity, Type entityType, IDAO dao)
        {
            InitializeComponent();
            this.entity = entity;
            this.entityType = entityType;
            this.dao = dao;
            GenerateFields();
        }

        private void GenerateFields()
        {
            int y = 10;
            var props = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var prop in props)
            {
                if (prop.GetCustomAttribute<IdAttribute>() != null) continue;

                var label = new Label
                {
                    Text = prop.Name,
                    Top = y,
                    Left = 10,
                    Width = 100
                };

                var textbox = CreateTextBox(prop.Name, prop.GetValue(entity)?.ToString(), y);

                Controls.Add(label);
                Controls.Add(textbox);

                y += 30;
            }

            var saveButton = new Button
            {
                Text = "Save",
                Top = y,
                Left = 10,
                Height = 30
            };
            saveButton.Click += EditButton_Click;
            Controls.Add(saveButton);

            var deleteButton = new Button
            {
                Text = "Delete",
                Top = y + 40,
                Left = 10,
                Height = 30
            };
            deleteButton.Click += DeleteButton_Click;
            Controls.Add(deleteButton);
        }

        private TextBox CreateTextBox(string name, string? text, int y)
        {
            return new TextBox
            {
                Name = name,
                Text = text ?? string.Empty,
                Top = y,
                Left = 120,
                Width = 200
            };
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            try
            {
                AssignTextBoxValuesToEntity();

                if (entityType == typeof(Bom))
                {
                    UpdateBomForeignKeys();
                }

                dao.Edit(entity);
                Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("Error while editing entity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AssignTextBoxValuesToEntity()
        {
            foreach (var prop in entityType.GetProperties())
            {
                if (Controls[prop.Name] is TextBox textBox)
                {
                    try
                    {
                        object convertedValue = Convert.ChangeType(textBox.Text, prop.PropertyType);
                        prop.SetValue(entity, convertedValue);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to assign value to {prop.Name}: {ex.Message}");
                    }
                }
            }
        }

        private void UpdateBomForeignKeys()
        {
            try
            {
                var bom = entity as Bom ?? throw new InvalidCastException("Entity is not of type Bom");

                var deviceDao = new DeviceDAO();
                bom.DeviceId = deviceDao.GetIdByName(bom.DeviceName);

                var partsDao = new PartsDAO();
                bom.PartId = partsDao.GetIdByName(bom.PartName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while updating foreign keys: " + ex.Message);
                MessageBox.Show("Error while updating related fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                dao.Delete(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("Error while deleting entity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Close();
        }
    }
}
