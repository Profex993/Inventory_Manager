using Inventory_Manager.Database;
using System.Text.Json;

namespace Inventory_Manager.Forms
{
    public partial class ConnectionForm : Form
    {
        public ConnectionForm()
        {
            InitializeComponent();

            if (File.Exists(PasswordStorage.filePath))
            {
                string json = File.ReadAllText(PasswordStorage.filePath);
                var settings = JsonSerializer.Deserialize<DbConnectionSettings>(json);

                string decryptedPassword = PasswordStorage.Decrypt(settings.EncryptedPassword);

                HostInput.Text = settings.Host;
                UsernameInput.Text = settings.Username;
                PasswordInput.Text = decryptedPassword;
            }
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (SaveCheckbox.Checked)
                {
                    var settings = new DbConnectionSettings
                    {
                        Host = HostInput.Text,
                        Username = UsernameInput.Text,
                        EncryptedPassword = PasswordStorage.Encrypt(PasswordInput.Text)
                    };

                    string json = JsonSerializer.Serialize(settings);
                    File.WriteAllText(PasswordStorage.filePath, json);
                }

                DatabaseConnection.TestConnection(HostInput.Text, UsernameInput.Text, PasswordInput.Text);

                _ = new DatabaseConnection(HostInput.Text, UsernameInput.Text, PasswordInput.Text);

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception)
            {
                Console.WriteLine("failed to connect to database");
                this.DialogResult = DialogResult.No;
            }

            Close();
        }
    }
}
