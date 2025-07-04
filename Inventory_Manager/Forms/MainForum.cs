﻿using Inventory_Manager.Database;
using Inventory_Manager.Database.DAO;
using Inventory_Manager.Entities;
using System.Collections;

namespace Inventory_Manager.Forms
{
    public partial class MainForum : Form
    {
        private IDAO currentDAO;
        private Type currentEntityType;
        public MainForum()
        {
            InitializeComponent();

            comboBox1.DataSource = TableManager.Instance.GetTableNames();

            dataView.ReadOnly = true;
            dataView.AllowUserToAddRows = false;
        }

        private void ComboBoxTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentDAO = TableManager.Instance.GetDAO(comboBox1.SelectedItem.ToString());
            currentEntityType = TableManager.Instance.GetEntityType(comboBox1.SelectedItem.ToString());

            LoadData();
        }

        public void DataView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && (currentEntityType != typeof(DeviceLog) && currentEntityType != typeof(PartLog)))
            {
                var rowObject = dataView.Rows[e.RowIndex].DataBoundItem;

                Entity entity = (Entity)Convert.ChangeType(rowObject, currentEntityType);

                var editForm = new EditEntityForm(entity, currentEntityType, currentDAO);
                editForm.ShowDialog();

                LoadData();
            }
        }

        private void LoadData()
        {
            List<Entity> baseEntities = currentDAO.GetAll();

            Type listType = typeof(List<>).MakeGenericType(currentEntityType);
            var typedList = (IList)Activator.CreateInstance(listType);

            foreach (var entity in baseEntities)
            {
                typedList.Add(entity);
            }

            dataView.DataSource = typedList;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (currentEntityType != typeof(DeviceLog) && currentEntityType != typeof(PartLog))
            {
                if (currentEntityType == typeof(Bom))
                {
                    new AddBomForm(currentDAO).ShowDialog();
                }
                else
                {
                    new AddEntityForm(currentEntityType, currentDAO).ShowDialog();
                }
                LoadData();
            }
        }

        private void BuildDeviceButton_Click(object sender, EventArgs e)
        {
            new BuildDeviceForum().ShowDialog();

            LoadData();
        }

        private void LogBrokenPartsButton_Click(object sender, EventArgs e)
        {
            new LogBrokenPartsForm().ShowDialog();

            LoadData();
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            new BOMcalculatorForm().ShowDialog();
        }
    }
}
