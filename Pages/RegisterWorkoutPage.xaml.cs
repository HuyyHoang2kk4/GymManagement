using GymManagement.Data;
using GymManagement.Models;
using Microsoft.Maui.Controls;
using System;
using System.Linq;

namespace GymManagement
{
    public partial class RegisterWorkoutPage : ContentPage
    {
        private readonly MemberRepository _memberRepository;
        private readonly WorkoutPackageRepository _packageRepository;
        private readonly WorkoutRegistrationRepository _registrationRepository;
        private string _currentPhoneNumber;

        public RegisterWorkoutPage()
        {
            InitializeComponent();
            _memberRepository = new MemberRepository();
            _packageRepository = new WorkoutPackageRepository();
            _registrationRepository = new WorkoutRegistrationRepository();

            // Khởi tạo danh sách gói tập
            var packages = _packageRepository.GetAllPackages();
            packagePicker.ItemsSource = packages.Select(p => p.PackageName).ToList();

            // Set ngày mặc định
            registrationDatePicker.Date = DateTime.Today;
        }

        private async void OnSearchClicked(object sender, EventArgs e)
        {
            _currentPhoneNumber = searchEntry.Text;

            if (string.IsNullOrEmpty(_currentPhoneNumber))
            {
                await DisplayAlert("Lỗi", "Vui lòng nhập số điện thoại", "OK");
                return;
            }

            // Tìm thành viên
            var member = _memberRepository.GetAllMembers()
                .FirstOrDefault(m => m.PhoneNumber == _currentPhoneNumber);

            if (member == null)
            {
                await DisplayAlert("Lỗi", "Không tìm thấy thành viên với số điện thoại này", "OK");
                memberInfoLayout.IsVisible = false;
                registrationFrame.IsVisible = false;
                return;
            }

            // Hiển thị thông tin thành viên
            memberNameLabel.Text = $"Họ tên: {member.FullName}";
            memberPhoneLabel.Text = $"SĐT: {member.PhoneNumber}";
            memberInfoLayout.IsVisible = true;

            // Hiển thị form đăng ký
            registrationFrame.IsVisible = true;

            // Load lịch sử đăng ký
            var registrations = _registrationRepository.GetRegistrationsByPhoneNumber(_currentPhoneNumber);
            registrationsCollectionView.ItemsSource = registrations;
        }

        private void OnPackageSelected(object sender, EventArgs e)
        {
            if (packagePicker.SelectedIndex == -1) return;

            var selectedPackageName = packagePicker.SelectedItem as string;
            var package = _packageRepository.GetAllPackages()
                .FirstOrDefault(p => p.PackageName == selectedPackageName);

            if (package != null)
            {
                var expirationDate = registrationDatePicker.Date.AddDays(package.DurationInDays);
                expirationDatePicker.Date = expirationDate;
            }
        }

        private async void OnConfirmRegisterClicked(object sender, EventArgs e)
        {
            if (packagePicker.SelectedIndex == -1)
            {
                await DisplayAlert("Lỗi", "Vui lòng chọn gói tập", "OK");
                return;
            }

            var selectedPackageName = packagePicker.SelectedItem as string;
            var package = _packageRepository.GetAllPackages()
                .FirstOrDefault(p => p.PackageName == selectedPackageName);

            var newRegistration = new WorkoutRegistration
            {
                PhoneNumber = _currentPhoneNumber,
                PackageId = package.Id,
                PackageName = package.PackageName,
                RegistrationDate = registrationDatePicker.Date,
                ExpirationDate = expirationDatePicker.Date
            };

            _registrationRepository.AddRegistration(newRegistration);
            await DisplayAlert("Thành công", "Đăng ký gói tập thành công", "OK");

            // Refresh danh sách
            var registrations = _registrationRepository.GetRegistrationsByPhoneNumber(_currentPhoneNumber);
            registrationsCollectionView.ItemsSource = registrations;
        }
    }
}