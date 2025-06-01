using SQLite;
using GymManagement.Models;
using System.Collections.Generic;
using System.IO;

namespace GymManagement.Data
{
    public class MemberRepository
    {
        private readonly SQLiteConnection _database;

        public MemberRepository()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "gymmanagement.db");
            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<Member>();
        }

        public List<Member> GetAllMembers()
        {
            return _database.Table<Member>().ToList();
        }

        public List<Member> SearchMembersByPhoneNumber(string phoneNumber)
        {
            return _database.Table<Member>().Where(m => m.PhoneNumber.Contains(phoneNumber)).ToList();
        }

        public void DeleteMember(int id)
        {
            var member = _database.Table<Member>().FirstOrDefault(m => m.Id == id);
            if (member != null)
            {
                _database.Delete(member);
            }
        }

        public void AddMember(Member member)
        {
            var existingMember = _database.Table<Member>().FirstOrDefault(m => m.PhoneNumber == member.PhoneNumber);
            if (existingMember != null)
            {
                throw new Exception("Số điện thoại đã tồn tại");
            }

            _database.Insert(member);
        }

        public void UpdateMember(Member member)
        {
            var existingMember = _database.Table<Member>().FirstOrDefault(m => m.Id == member.Id);
            if (existingMember != null)
            {
                _database.Update(member);
            }
        }
    }
}
