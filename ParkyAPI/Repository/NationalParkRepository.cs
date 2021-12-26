using System.Collections.Generic;
using System.Linq;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _db;

        public NationalParkRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return _db.NationalParks.OrderBy(p => p.Name).ToList();
        }

        public NationalPark GetNationalPark(int id)
        {
            return _db.NationalParks.Find(id);
        }

        public bool NationalParkExists(string name)
        {
            return _db.NationalParks.Any(p => p.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool NationalParkExists(int id)
        {
            return _db.NationalParks.Any(p => p.Id == id);
        }

        public bool CreateNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Update(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _db.NationalParks.Remove(nationalPark);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }
    }
}