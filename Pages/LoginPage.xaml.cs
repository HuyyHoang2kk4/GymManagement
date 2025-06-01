namespace GymManagement;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string username = usernameEntry.Text;
        string password = passwordEntry.Text;

        if (username == "admin" && password == "123") // Đăng nhập đúng
        {
            await Navigation.PushAsync(new MainPage()); // Hoặc Navigation.PushModalAsync(...)
        }
        else
        {
            // Đăng nhập sai → hiện popup như ảnh
            await DisplayAlert("Lỗi", "Tên đăng nhập hoặc mật khẩu không đúng", "OK");
        }
    }

}
