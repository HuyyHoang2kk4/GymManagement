using SQLite;

namespace GymManagement.Models
{
    public class WorkoutPackage
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string PackageName { get; set; }

        // Số ngày áp dụng cho gói tập
        public int DurationInDays { get; set; }

        public decimal Price { get; set; }
    }
}
