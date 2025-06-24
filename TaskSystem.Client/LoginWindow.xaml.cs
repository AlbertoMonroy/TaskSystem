using System.Linq;
using System.Windows;
using TaskSystem.Client.Models;

namespace TaskSystem.Client
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password;

            var user = await ApiService.Instance.LoginAsync(username, password);

            if (user == null)
            {
                MessageBox.Show("Credenciales incorrectas");
                return;
            }

            App.Current.Properties["LoggedUser"] = user;

            DialogResult = true;
            Close();
        }
    }
}
