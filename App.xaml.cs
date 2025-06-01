using Microsoft.Maui.Controls;
using GymManagement.Data;

namespace GymManagement
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            // Thay đổi cách tạo cửa sổ, sử dụng CreateWindow thay vì set MainPage
            MainPage = new NavigationPage(new MainPage());
        }
    }
}