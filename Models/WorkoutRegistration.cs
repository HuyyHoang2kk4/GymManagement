using SQLite;

namespace GymManagement.Models
{
    public class WorkoutRegistration
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string PhoneNumber { get; set; } // Thêm thuộc tính này nếu chưa có
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}