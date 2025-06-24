using System.Windows;

namespace TaskSystem.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var loginWindow = new LoginWindow();
            var result = loginWindow.ShowDialog();

            if (result == true)
            {
                var mainWindow = new MainWindow();
                Application.Current.MainWindow = mainWindow;
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                MessageBox.Show("Login exitoso. Abriendo MainWindow...");
                mainWindow.Show();
            }
            else
            {
                MessageBox.Show("Login fallido o cancelado.");
                Shutdown();
            }
        }
    }
}
