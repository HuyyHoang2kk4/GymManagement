using SQLite;
using GymManagement.Models;
using System.Collections.Generic;
using System.IO;

namespace GymManagement.Data
{
    public class WorkoutRegistrationRepository
    {
        private readonly SQLiteConnection _database;

        public WorkoutRegistrationRepository()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "gymmanagement.db");
            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<WorkoutRegistration>();
        }

        public List<WorkoutRegistration> GetRegistrationsByPhoneNumber(string phoneNumber)
        {
            return _database.Table<WorkoutRegistration>()
                          .Where(r => r.PhoneNumber == phoneNumber)
                          .ToList();
        }

        public void AddRegistration(WorkoutRegistration registration)
        {
            _database.Insert(registration);
        }

        public List<WorkoutRegistration> GetAllRegistrations()
        {
            return _database.Table<WorkoutRegistration>().ToList();
        }
    }
}