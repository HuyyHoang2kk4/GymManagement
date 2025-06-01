using SQLite;
using GymManagement.Models;
using System.Collections.Generic;
using System.IO;

namespace GymManagement.Data
{
    public class WorkoutPackageRepository
    {
        private readonly SQLiteConnection _database;

        public WorkoutPackageRepository()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "gymmanagement.db");
            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<WorkoutPackage>();
        }

        public List<WorkoutPackage> GetAllPackages()
        {
            return _database.Table<WorkoutPackage>().ToList();
        }

        public void AddPackage(WorkoutPackage package)
        {
            _database.Insert(package);
        }

        public void UpdatePackage(WorkoutPackage package)
        {
            _database.Update(package);
        }

        public void DeletePackage(int id)
        {
            var package = _database.Table<WorkoutPackage>().FirstOrDefault(p => p.Id == id);
            if (package != null)
            {
                _database.Delete(package);
            }
        }

        public List<WorkoutPackage> SearchPackagesByName(string packageName)
        {
            return _database.Table<WorkoutPackage>().Where(p => p.PackageName.Contains(packageName)).ToList();
        }
    }
}
