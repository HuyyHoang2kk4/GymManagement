using SQLite;

namespace GymManagement.Models
{
    public class Member
    {
        [PrimaryKey, AutoIncrement]  // Sử dụng SQLite attribute
        public int Id { get; set; }

        [NotNull]  // Sử dụng SQLite attribute
        public string FullName { get; set; }

        [NotNull]  // Sử dụng SQLite attribute
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
    }
}
