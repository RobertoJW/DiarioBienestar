using Microsoft.Maui.Controls.Compatibility.Platform;

namespace DiarioBienestar
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
