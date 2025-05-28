namespace Inventory_Manager.Forms
{
    partial class BuildDeviceForum
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
            DeviceName = new ComboBox();
            Quantity = new NumericUpDown();
            DeviceLabel = new Label();
            QuantityLabel = new Label();
            SaveButton = new Button();
            ((System.ComponentModel.ISupportInitialize)Quantity).BeginInit();
            SuspendLayout();
            // 
            // DeviceName
            // 
            DeviceName.FormattingEnabled = true;
            DeviceName.Location = new Point(114, 44);
            DeviceName.Name = "DeviceName";
            DeviceName.Size = new Size(151, 28);
            DeviceName.TabIndex = 0;
            // 
            // Quantity
            // 
            Quantity.Location = new Point(114, 78);
            Quantity.Name = "Quantity";
            Quantity.Size = new Size(150, 27);
            Quantity.TabIndex = 1;
            // 
            // DeviceLabel
            // 
            DeviceLabel.AutoSize = true;
            DeviceLabel.Location = new Point(13, 47);
            DeviceLabel.Name = "DeviceLabel";
            DeviceLabel.Size = new Size(95, 20);
            DeviceLabel.TabIndex = 2;
            DeviceLabel.Text = "Device name";
            // 
            // QuantityLabel
            // 
            QuantityLabel.AutoSize = true;
            QuantityLabel.Location = new Point(13, 78);
            QuantityLabel.Name = "QuantityLabel";
            QuantityLabel.Size = new Size(65, 20);
            QuantityLabel.TabIndex = 3;
            QuantityLabel.Text = "Quantity";
            // 
            // SaveButton
            // 
            SaveButton.Location = new Point(99, 124);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(94, 29);
            SaveButton.TabIndex = 4;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // BuildDeviceForum
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(302, 226);
            Controls.Add(SaveButton);
            Controls.Add(QuantityLabel);
            Controls.Add(DeviceLabel);
            Controls.Add(Quantity);
            Controls.Add(DeviceName);
            Name = "BuildDeviceForum";
            Text = "Build device";
            ((System.ComponentModel.ISupportInitialize)Quantity).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox DeviceName;
        private NumericUpDown Quantity;
        private Label DeviceLabel;
        private Label QuantityLabel;
        private Button SaveButton;
    }
}