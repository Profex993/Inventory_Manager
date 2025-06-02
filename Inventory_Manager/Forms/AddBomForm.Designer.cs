namespace Inventory_Manager.Forms
{
    partial class AddBomForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGrid = new DataGridView();
            deviceComboBox = new ComboBox();
            deviceLabel = new Label();
            saveButton = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGrid).BeginInit();
            SuspendLayout();
            // 
            // dataGrid
            // 
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGrid.Location = new Point(12, 70);
            dataGrid.Name = "dataGrid";
            dataGrid.RowHeadersWidth = 51;
            dataGrid.Size = new Size(597, 330);
            dataGrid.TabIndex = 0;
            // 
            // deviceComboBox
            // 
            deviceComboBox.FormattingEnabled = true;
            deviceComboBox.Location = new Point(73, 26);
            deviceComboBox.Name = "deviceComboBox";
            deviceComboBox.Size = new Size(124, 28);
            deviceComboBox.TabIndex = 1;
            // 
            // deviceLabel
            // 
            deviceLabel.AutoSize = true;
            deviceLabel.Location = new Point(12, 29);
            deviceLabel.Name = "deviceLabel";
            deviceLabel.Size = new Size(55, 20);
            deviceLabel.TabIndex = 2;
            deviceLabel.Text = "device:";
            // 
            // saveButton
            // 
            saveButton.Location = new Point(12, 406);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(94, 29);
            saveButton.TabIndex = 3;
            saveButton.Text = "save all";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += SaveButton_Click;
            // 
            // AddBomForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(621, 450);
            Controls.Add(saveButton);
            Controls.Add(deviceLabel);
            Controls.Add(deviceComboBox);
            Controls.Add(dataGrid);
            Name = "AddBomForm";
            Text = "AddBomForm";
            ((System.ComponentModel.ISupportInitialize)dataGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGrid;
        private ComboBox deviceComboBox;
        private Label deviceLabel;
        private Button saveButton;
    }
}