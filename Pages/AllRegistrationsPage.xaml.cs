using GymManagement.Data;
using GymManagement.Models;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;

namespace GymManagement
{
    public partial class AllRegistrationsPage : ContentPage
    {
        private readonly WorkoutRegistrationRepository _registrationRepository;
        private readonly MemberRepository _memberRepository;
        private readonly WorkoutPackageRepository _packageRepository;

        public AllRegistrationsPage()
        {
            InitializeComponent();
            _registrationRepository = new WorkoutRegistrationRepository();
            _memberRepository = new MemberRepository();
            _packageRepository = new WorkoutPackageRepository();

            LoadAllRegistrations();
        }

        private void LoadAllRegistrations(string searchTerm = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var allRegistrations = _registrationRepository.GetAllRegistrations();
            var members = _memberRepository.GetAllMembers();
            var packages = _packageRepository.GetAllPackages();

            var registrationsWithMemberInfo = allRegistrations.Select(r =>
            {
                var member = members.FirstOrDefault(m => m.PhoneNumber == r.PhoneNumber);
                var package = packages.FirstOrDefault(p => p.PackageName == r.PackageName);
                return new RegistrationViewModel
                {
                    Id = r.Id,
                    PhoneNumber = r.PhoneNumber,
                    MemberFullName = member?.FullName ?? "Không xác định",
                    PackageName = r.PackageName,
                    RegistrationDate = r.RegistrationDate,
                    ExpirationDate = r.ExpirationDate,
                    Price = package?.Price ?? 0
                };
            });

            if (!string.IsNullOrEmpty(searchTerm))
            {
                registrationsWithMemberInfo = registrationsWithMemberInfo
                    .Where(r => r.PhoneNumber.Contains(searchTerm) ||
                               r.MemberFullName.Contains(searchTerm));
            }

            if (startDate.HasValue)
            {
                registrationsWithMemberInfo = registrationsWithMemberInfo
                    .Where(r => r.RegistrationDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                registrationsWithMemberInfo = registrationsWithMemberInfo
                    .Where(r => r.RegistrationDate <= endDate.Value);
            }

            registrationsCollectionView.ItemsSource = registrationsWithMemberInfo.ToList();

            // Calculate total revenue
            var totalRevenue = registrationsWithMemberInfo.Sum(r => r.Price);
            revenueLabel.Text = $"{totalRevenue:N0} VND"; // Format the revenue as currency
        }

        private void OnSearchClicked(object sender, EventArgs e)
        {
            LoadAllRegistrations(searchEntry.Text, startDatePicker.Date, endDatePicker.Date);
        }

        private void OnRefreshClicked(object sender, EventArgs e)
        {
            searchEntry.Text = string.Empty;
            startDatePicker.Date = DateTime.Now;
            endDatePicker.Date = DateTime.Now;
            LoadAllRegistrations();
        }

        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            // Recalculate the revenue when a daate is selected
            LoadAllRegistrations(searchEntry.Text, startDatePicker.Date, endDatePicker.Date);
        }
    }

    public class RegistrationViewModel
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string MemberFullName { get; set; }
        public string PackageName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal Price { get; set; } // Add Price property to store the package price
    }
}
