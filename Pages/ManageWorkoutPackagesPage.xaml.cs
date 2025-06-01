using GymManagement.Data;
using GymManagement.Models;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;

namespace GymManagement
{
    public partial class ManageWorkoutPackagesPage : ContentPage
    {
        private readonly WorkoutPackageRepository _repository;
        public ObservableCollection<WorkoutPackage> Packages { get; set; }

        public ManageWorkoutPackagesPage()
        {
            InitializeComponent();
            _repository = new WorkoutPackageRepository();
            Packages = new ObservableCollection<WorkoutPackage>(_repository.GetAllPackages());
            packagesCollectionView.ItemsSource = Packages;
        }

        private void OnSearchClicked(object sender, EventArgs e)
        {
            var packageName = searchEntry.Text;

            if (!string.IsNullOrEmpty(packageName))
            {
                packagesCollectionView.ItemsSource = new ObservableCollection<WorkoutPackage>(_repository.SearchPackagesByName(packageName));
            }
            else
            {
                packagesCollectionView.ItemsSource = Packages;
            }
        }

        private async void OnAddPackageClicked(object sender, EventArgs e)
        {
            string packageName = await DisplayPromptAsync("Nhập tên gói", "Nhập tên gói tập");
            string priceStr = await DisplayPromptAsync("Nhập giá", "Nhập giá gói tập");
            string durationStr = await DisplayPromptAsync("Nhập số ngày", "Nhập số ngày của gói tập");

            if (string.IsNullOrEmpty(packageName) || string.IsNullOrEmpty(priceStr) || string.IsNullOrEmpty(durationStr))
            {
                await DisplayAlert("Lỗi", "Tên gói, giá và số ngày là bắt buộc", "OK");
                return;
            }

            try
            {
                decimal price = decimal.Parse(priceStr);
                int durationInDays = int.Parse(durationStr);
                WorkoutPackage newPackage = new WorkoutPackage
                {
                    PackageName = packageName,
                    Price = price,
                    DurationInDays = durationInDays
                };

                _repository.AddPackage(newPackage);
                Packages.Add(newPackage);
                await DisplayAlert("Thông báo", "Thêm gói tập thành công", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", ex.Message, "OK");
            }
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var package = button?.BindingContext as WorkoutPackage;

            if (package != null)
            {
                string newPackageName = await DisplayPromptAsync("Sửa tên gói", "Nhập tên gói tập mới", initialValue: package.PackageName);
                string newPriceStr = await DisplayPromptAsync("Sửa giá", "Nhập giá mới", initialValue: package.Price.ToString());
                string newDurationStr = await DisplayPromptAsync("Sửa số ngày", "Nhập số ngày mới", initialValue: package.DurationInDays.ToString());

                if (string.IsNullOrEmpty(newPackageName) || string.IsNullOrEmpty(newPriceStr) || string.IsNullOrEmpty(newDurationStr))
                {
                    await DisplayAlert("Lỗi", "Tên gói, giá và số ngày là bắt buộc", "OK");
                    return;
                }

                decimal newPrice = decimal.Parse(newPriceStr);
                int newDurationInDays = int.Parse(newDurationStr);
                WorkoutPackage updatedPackage = new WorkoutPackage
                {
                    Id = package.Id,
                    PackageName = newPackageName,
                    Price = newPrice,
                    DurationInDays = newDurationInDays
                };

                _repository.UpdatePackage(updatedPackage);
                var index = Packages.IndexOf(package);
                Packages[index] = updatedPackage;
                await DisplayAlert("Thông báo", "Cập nhật gói tập thành công", "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var package = button?.BindingContext as WorkoutPackage;

            if (package != null)
            {
                var confirm = await DisplayAlert("Xóa Gói Tập", $"Bạn có chắc chắn muốn xóa gói tập {package.PackageName}?", "Xóa", "Hủy");
                if (confirm)
                {
                    _repository.DeletePackage(package.Id);
                    Packages.Remove(package);
                }
            }
        }
    }
}
