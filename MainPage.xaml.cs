using Microsoft.Maui.Controls;

namespace GymManagement
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // Sự kiện khi nhấn vào nút Quản lý thành viên
        private async void OnManageMembersClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageMembersPage());
        }

        // Sự kiện cho các nút khác (có thể để tạm thời)
        private async void OnManagePackagesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageWorkoutPackagesPage());
        }


        private async void OnManageRegistrationsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterWorkoutPage());
        }

        private async void OnManageRevenueClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AllRegistrationsPage());
        }

        //private async void OnLogoutClicked(object sender, EventArgs e)
        //{
        //    bool answer = await DisplayAlert("Xác nhận", "Bạn có chắc chắn muốn đăng xuất?", "Có", "Không");

        //    if (answer)
        //    {
        //        // Không chuyển trang hay xóa session ở đây
        //        await DisplayAlert("Thông báo", "Bạn đã đăng xuất", "OK");
        //    }
        //    // Nếu chọn "Không", hộp thoại sẽ tự đóng mà không làm gì thêm
        //}

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Xác nhận", "Bạn có chắc chắn muốn đăng xuất?", "Có", "Không");

            if (answer)
            {
                // Điều hướng về LoginPage
                Application.Current.MainPage = new LoginPage();
            }
        }


    }
}
