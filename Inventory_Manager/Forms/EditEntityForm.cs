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

            var props = entityType.GetProperties((BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));

            foreach (var prop in props)
            {
                if (prop.GetCustomAttribute<IdAttribute>() == null)
                {
                    var label = new Label { Text = prop.Name, Top = y, Left = 10, Width = 100 };
                    var textbox = new TextBox
                    {
                        Name = prop.Name,
                        Top = y,
                        Left = 120,
                        Width = 200,
                        Text = prop.GetValue(entity)?.ToString()
                    };
                    this.Controls.Add(label);
                    this.Controls.Add(textbox);
                    y += 30;
                }
            }

            var editButton = new Button { Text = "Save", Top = y, Left = 10, Height = 30 };
            editButton.Click += EditButton_Click;
            this.Controls.Add(editButton);

            var deleteButton = new Button { Text = "Delete", Top = y + 30, Left = 10, Height = 30 };
            deleteButton.Click += DeleteButton_Click;
            this.Controls.Add(deleteButton);
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            foreach (var prop in entity.GetType().GetProperties())
            {
                if (this.Controls[prop.Name] is TextBox control)
                {
                    object convertedValue = Convert.ChangeType(control.Text, prop.PropertyType);
                    prop.SetValue(entity, convertedValue);
                }
            }

            if (entityType == typeof(Bom))
            {
                try
                {
                    DeviceDAO deviceDao = new();
                    (entity as Bom).DeviceId = deviceDao.GetIdByName((entity as Bom).DeviceName);
                }
                catch (Exception)
                {
                    this.Close();
                    Console.WriteLine("Error while getting device name.");
                    MessageBox.Show("Error while getting device name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                try
                {
                    PartsDAO partsDAO = new();
                    (entity as Bom).PartId = partsDAO.GetIdByName((entity as Bom).PartName);
                }
                catch (Exception)
                {
                    this.Close();
                    Console.WriteLine("Error while getting part name.");
                    MessageBox.Show("Error while getting part name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            try
            {
                dao.Edit(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("Error while editing entity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Close();
        }

        private void DeleteButton_Click(object sendre, EventArgs e)
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

            this.Close();
        }
    }
}
