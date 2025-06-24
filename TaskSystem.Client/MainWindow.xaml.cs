using System.Windows;
using System.Windows.Controls;
using TaskSystem.Client.Models;

namespace TaskSystem.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (App.Current.Properties["LoggedUser"] is UserModel user)
            {
                this.Title = $"Task System App - Bienvenido, {user.FullName}";
            }
        }

        private void TasksGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (e.Row.Item is TaskModel task)
            {
                if (task.IsLocked)
                {
                    MessageBox.Show("Esta tarea está siendo editada por otro usuario.");
                    e.Cancel = true;
                    return;
                }

                var userId = ((UserModel)App.Current.Properties["LoggedUser"]).Id;
                _ = SignalRService.Instance.SendTaskLock(task.Id, userId);
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            ApiService.Instance.ClearSession();
            App.Current.Properties["LoggedUser"] = null;

            var loginWindow = new LoginWindow();
            var result = loginWindow.ShowDialog();

            this.Close();

            if (result == true)
            {
                var nuevaMain = new MainWindow();
                nuevaMain.Show();
            }

            
        }
    }
}
