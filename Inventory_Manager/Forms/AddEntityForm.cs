using Inventory_Manager.Database.DAO;
using Inventory_Manager.Entities;
using System.Reflection;

namespace Inventory_Manager.Forms
{
    public partial class AddEntityForm : Form
    {
        private readonly Type entityType;
        private readonly IDAO dao;

        public AddEntityForm(Type entityType, IDAO dao)
        {
            this.entityType = entityType;
            this.dao = dao;
            InitializeComponent();
            GenerateFields();
        }

        private void GenerateFields()
        {
            int y = 10;
            var props = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var prop in props)
            {
                if (prop.GetCustomAttribute<IdAttribute>() != null) continue;

                Control inputControl = null;

                if (entityType == typeof(Bom))
                {
                    inputControl = prop.Name switch
                    {
                        "DeviceName" => CreateComboBox(prop.Name, y, [.. new DeviceDAO().GetAllNames()]),
                        "PartName" => CreateComboBox(prop.Name, y, [.. new PartsDAO().GetAllNames()]),
                        _ => CreateTextBox(prop.Name, y)
                    };
                }
                else
                {
                    inputControl = CreateTextBox(prop.Name, y);
                }

                var label = new Label
                {
                    Text = prop.Name,
                    Top = y,
                    Left = 10,
                    Width = 100
                };

                Controls.Add(label);
                Controls.Add(inputControl);
                y += 30;
            }

            var saveButton = new Button
            {
                Text = "Save",
                Top = y,
                Left = 10,
                Height = 30
            };
            saveButton.Click += SaveButton_Click;
            Controls.Add(saveButton);
        }

        private ComboBox CreateComboBox(string name, int y, string[] items)
        {
            var comboBox = new ComboBox
            {
                Name = name,
                Top = y,
                Left = 120,
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            comboBox.Items.AddRange(items);
            return comboBox;
        }

        private TextBox CreateTextBox(string name, int y)
        {
            return new TextBox
            {
                Name = name,
                Top = y,
                Left = 120,
                Width = 200
            };
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                object entity = Activator.CreateInstance(entityType)!;

                foreach (var prop in entityType.GetProperties())
                {
                    if (!Controls.ContainsKey(prop.Name)) continue;

                    var control = Controls[prop.Name];

                    switch (control)
                    {
                        case ComboBox comboBox:
                            HandleComboBoxAssignment(entity, prop, comboBox);
                            break;

                        case TextBox textBox:
                            AssignValueToProperty(entity, prop, textBox.Text);
                            break;
                    }
                }

                dao.Add(entity as Entity);
                Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("Error while adding entity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleComboBoxAssignment(object entity, PropertyInfo prop, ComboBox comboBox)
        {
            try
            {
                string selected = comboBox.Text;

                if (entityType == typeof(Bom))
                {
                    switch (prop.Name)
                    {
                        case "DeviceName":
                            SetForeignKey(entity, "DeviceId", new DeviceDAO().GetIdByName(selected));
                            break;
                        case "PartName":
                            SetForeignKey(entity, "PartId", new PartsDAO().GetIdByName(selected));
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to resolve foreign key for {prop.Name}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine($"Error while resolving foreign key for {prop.Name}: {ex}");
                Close();
            }
        }

        private void SetForeignKey(object entity, string targetPropName, object idValue)
        {
            var targetProp = entity.GetType().GetProperty(targetPropName);
            if (targetProp != null)
            {
                object convertedValue = Convert.ChangeType(idValue, targetProp.PropertyType);
                targetProp.SetValue(entity, convertedValue);
            }
        }

        private void AssignValueToProperty(object entity, PropertyInfo prop, string text)
        {
            try
            {
                object convertedValue = Convert.ChangeType(text, prop.PropertyType);
                prop.SetValue(entity, convertedValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to convert and set {prop.Name}: {ex.Message}");
            }
        }
    }
}
