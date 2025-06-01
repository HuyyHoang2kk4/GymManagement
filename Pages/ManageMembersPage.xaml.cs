using GymManagement.Data;
using GymManagement.Models;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;

namespace GymManagement
{
    public partial class ManageMembersPage : ContentPage
    {
        private readonly MemberRepository _repository;
        public ObservableCollection<Member> Members { get; set; }

        public ManageMembersPage()
        {
            InitializeComponent();
            _repository = new MemberRepository();
            Members = new ObservableCollection<Member>(_repository.GetAllMembers());
            membersCollectionView.ItemsSource = Members;
        }

        private void OnSearchClicked(object sender, EventArgs e)
        {
            var phoneNumber = searchEntry.Text;

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                membersCollectionView.ItemsSource = new ObservableCollection<Member>(_repository.SearchMembersByPhoneNumber(phoneNumber));
            }
            else
            {
                membersCollectionView.ItemsSource = Members;
            }
        }

        private async void OnAddMemberClicked(object sender, EventArgs e)
        {
            string fullName = await DisplayPromptAsync("Nhập tên", "Nhập tên thành viên");
            string phoneNumber = await DisplayPromptAsync("Nhập số điện thoại", "Nhập số điện thoại thành viên");
            string address = await DisplayPromptAsync("Nhập địa chỉ", "Nhập địa chỉ thành viên");

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(phoneNumber))
            {
                await DisplayAlert("Lỗi", "Tên và số điện thoại là bắt buộc", "OK");
                return;
            }

            try
            {
                Member newMember = new Member
                {
                    FullName = fullName,
                    PhoneNumber = phoneNumber,
                    Address = address
                };

                _repository.AddMember(newMember);
                Members.Add(newMember);
                await DisplayAlert("Thông báo", "Thêm thành viên thành công", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", ex.Message, "OK");
            }
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var member = button?.BindingContext as Member;

            if (member != null)
            {
                string newFullName = await DisplayPromptAsync("Sửa tên", "Nhập tên thành viên mới", initialValue: member.FullName);
                string newPhoneNumber = await DisplayPromptAsync("Sửa số điện thoại", "Nhập số điện thoại mới", initialValue: member.PhoneNumber);
                string newAddress = await DisplayPromptAsync("Sửa địa chỉ", "Nhập địa chỉ mới", initialValue: member.Address);

                if (string.IsNullOrEmpty(newFullName) || string.IsNullOrEmpty(newPhoneNumber))
                {
                    await DisplayAlert("Lỗi", "Tên và số điện thoại là bắt buộc", "OK");
                    return;
                }

                Member updatedMember = new Member
                {
                    Id = member.Id,
                    FullName = newFullName,
                    PhoneNumber = newPhoneNumber,
                    Address = newAddress
                };

                _repository.UpdateMember(updatedMember);
                var index = Members.IndexOf(member);
                Members[index] = updatedMember;
                await DisplayAlert("Thông báo", "Cập nhật thành viên thành công", "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var member = button?.BindingContext as Member;

            if (member != null)
            {
                var confirm = await DisplayAlert("Xóa Thành Viên", $"Bạn có chắc chắn muốn xóa thành viên {member.FullName}?", "Xóa", "Hủy");
                if (confirm)
                {
                    _repository.DeleteMember(member.Id);
                    Members.Remove(member);
                }
            }
        }
    }
}
