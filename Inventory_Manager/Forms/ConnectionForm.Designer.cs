namespace Inventory_Manager.Forms
{
    partial class ConnectionForm
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
            label1 = new Label();
            HostInput = new TextBox();
            UsernameInput = new TextBox();
            PasswordInput = new TextBox();
            label2 = new Label();
            label4 = new Label();
            label5 = new Label();
            ConnectButton = new Button();
            SaveCheckbox = new CheckBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(56, 9);
            label1.Name = "label1";
            label1.Size = new Size(213, 20);
            label1.TabIndex = 0;
            label1.Text = "Connect to Postgres SQL server";
            // 
            // HostInput
            // 
            HostInput.Location = new Point(103, 74);
            HostInput.Name = "HostInput";
            HostInput.Size = new Size(125, 27);
            HostInput.TabIndex = 1;
            // 
            // UsernameInput
            // 
            UsernameInput.Location = new Point(103, 107);
            UsernameInput.Name = "UsernameInput";
            UsernameInput.Size = new Size(125, 27);
            UsernameInput.TabIndex = 2;
            // 
            // PasswordInput
            // 
            PasswordInput.Location = new Point(103, 140);
            PasswordInput.Name = "PasswordInput";
            PasswordInput.PasswordChar = '*';
            PasswordInput.Size = new Size(125, 27);
            PasswordInput.TabIndex = 3;
            PasswordInput.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(41, 78);
            label2.Name = "label2";
            label2.Size = new Size(43, 20);
            label2.TabIndex = 5;
            label2.Text = "Host:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(22, 110);
            label4.Name = "label4";
            label4.Size = new Size(75, 20);
            label4.TabIndex = 7;
            label4.Text = "Username";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(24, 143);
            label5.Name = "label5";
            label5.Size = new Size(73, 20);
            label5.TabIndex = 8;
            label5.Text = "Password:";
            // 
            // ConnectButton
            // 
            ConnectButton.Location = new Point(112, 192);
            ConnectButton.Name = "ConnectButton";
            ConnectButton.Size = new Size(94, 29);
            ConnectButton.TabIndex = 9;
            ConnectButton.Text = "Connect";
            ConnectButton.UseVisualStyleBackColor = true;
            ConnectButton.Click += ConnectButton_Click;
            // 
            // SaveCheckbox
            // 
            SaveCheckbox.AutoSize = true;
            SaveCheckbox.Location = new Point(212, 195);
            SaveCheckbox.Name = "SaveCheckbox";
            SaveCheckbox.Size = new Size(60, 24);
            SaveCheckbox.TabIndex = 10;
            SaveCheckbox.Text = "save";
            SaveCheckbox.UseVisualStyleBackColor = true;
            // 
            // ConnectionForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(335, 311);
            Controls.Add(SaveCheckbox);
            Controls.Add(ConnectButton);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(PasswordInput);
            Controls.Add(UsernameInput);
            Controls.Add(HostInput);
            Controls.Add(label1);
            Name = "ConnectionForm";
            Text = "Inventory Manager";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox HostInput;
        private TextBox UsernameInput;
        private TextBox PasswordInput;
        private Label label2;
        private Label label4;
        private Label label5;
        private Button ConnectButton;
        private CheckBox SaveCheckbox;
    }
}