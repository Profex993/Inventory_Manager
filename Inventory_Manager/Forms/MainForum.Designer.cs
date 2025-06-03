namespace Inventory_Manager.Forms
{
    partial class MainForum
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
            dataView = new DataGridView();
            comboBox1 = new ComboBox();
            addButton = new Button();
            buildButton = new Button();
            brokenPartButton = new Button();
            calculateButton = new Button();
            ((System.ComponentModel.ISupportInitialize)dataView).BeginInit();
            SuspendLayout();
            // 
            // dataView
            // 
            dataView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataView.Location = new Point(12, 46);
            dataView.Name = "dataView";
            dataView.RowHeadersWidth = 51;
            dataView.Size = new Size(1315, 734);
            dataView.TabIndex = 0;
            dataView.CellDoubleClick += DataView_CellDoubleClick;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(12, 12);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(151, 28);
            comboBox1.TabIndex = 1;
            comboBox1.SelectedIndexChanged += ComboBoxTables_SelectedIndexChanged;
            // 
            // addButton
            // 
            addButton.Location = new Point(169, 11);
            addButton.Name = "addButton";
            addButton.Size = new Size(94, 29);
            addButton.TabIndex = 2;
            addButton.Text = "add";
            addButton.UseVisualStyleBackColor = true;
            addButton.Click += AddButton_Click;
            // 
            // buildButton
            // 
            buildButton.Location = new Point(269, 11);
            buildButton.Name = "buildButton";
            buildButton.Size = new Size(131, 29);
            buildButton.TabIndex = 3;
            buildButton.Text = "build device";
            buildButton.UseVisualStyleBackColor = true;
            buildButton.Click += BuildDeviceButton_Click;
            // 
            // brokenPartButton
            // 
            brokenPartButton.Location = new Point(406, 12);
            brokenPartButton.Name = "brokenPartButton";
            brokenPartButton.Size = new Size(136, 29);
            brokenPartButton.TabIndex = 4;
            brokenPartButton.Text = "log broken parts";
            brokenPartButton.UseVisualStyleBackColor = true;
            brokenPartButton.Click += LogBrokenPartsButton_Click;
            // 
            // calculateButton
            // 
            calculateButton.Location = new Point(548, 11);
            calculateButton.Name = "calculateButton";
            calculateButton.Size = new Size(123, 29);
            calculateButton.TabIndex = 5;
            calculateButton.Text = "calculate BOM";
            calculateButton.UseVisualStyleBackColor = true;
            calculateButton.Click += CalculateButton_Click;
            // 
            // MainForum
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1339, 843);
            Controls.Add(calculateButton);
            Controls.Add(brokenPartButton);
            Controls.Add(buildButton);
            Controls.Add(addButton);
            Controls.Add(comboBox1);
            Controls.Add(dataView);
            Name = "MainForum";
            Text = "Inventory manager";
            ((System.ComponentModel.ISupportInitialize)dataView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataView;
        private ComboBox comboBox1;
        private Button addButton;
        private Button buildButton;
        private Button brokenPartButton;
        private Button calculateButton;
    }
}