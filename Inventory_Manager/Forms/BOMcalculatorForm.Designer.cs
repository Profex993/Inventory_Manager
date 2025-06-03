namespace Inventory_Manager.Forms
{
    partial class BOMcalculatorForm
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
            quantityInput = new NumericUpDown();
            runButton = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)quantityInput).BeginInit();
            SuspendLayout();
            // 
            // dataGrid
            // 
            dataGrid.AllowUserToAddRows = false;
            dataGrid.AllowUserToDeleteRows = false;
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGrid.Location = new Point(12, 45);
            dataGrid.Name = "dataGrid";
            dataGrid.ReadOnly = true;
            dataGrid.RowHeadersWidth = 51;
            dataGrid.Size = new Size(599, 393);
            dataGrid.TabIndex = 0;
            // 
            // deviceComboBox
            // 
            deviceComboBox.FormattingEnabled = true;
            deviceComboBox.Location = new Point(12, 12);
            deviceComboBox.Name = "deviceComboBox";
            deviceComboBox.Size = new Size(151, 28);
            deviceComboBox.TabIndex = 1;
            deviceComboBox.Text = "select device";
            // 
            // quantityInput
            // 
            quantityInput.Location = new Point(169, 12);
            quantityInput.Name = "quantityInput";
            quantityInput.Size = new Size(61, 27);
            quantityInput.TabIndex = 2;
            // 
            // runButton
            // 
            runButton.Location = new Point(517, 10);
            runButton.Name = "runButton";
            runButton.Size = new Size(94, 29);
            runButton.TabIndex = 3;
            runButton.Text = "calculate";
            runButton.UseVisualStyleBackColor = true;
            runButton.Click += CalculateButton_Click;
            // 
            // BOMcalculatorForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(623, 450);
            Controls.Add(runButton);
            Controls.Add(quantityInput);
            Controls.Add(deviceComboBox);
            Controls.Add(dataGrid);
            Name = "BOMcalculatorForm";
            Text = "BOM calculator";
            ((System.ComponentModel.ISupportInitialize)dataGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)quantityInput).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGrid;
        private ComboBox deviceComboBox;
        private NumericUpDown quantityInput;
        private Button runButton;
    }
}