using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;

namespace ParkyAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _db;

        public TrailRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trails.Include(t => t.NationalPark).OrderBy(t => t.Name).ToList();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int id)
        {
            return _db.Trails
                .Include(t => t.NationalPark)
                .Where(t => t.NationalParkId == id)
                .OrderBy(t => t.Name)
                .ToList();
        }

        public Trail GetTrail(int id)
        {
            return _db.Trails.Include(t => t.NationalPark).FirstOrDefault(t => t.Id == id);
        }

        public bool TrailExists(string name)
        {
            return _db.Trails.Any(p => p.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool TrailExists(int id)
        {
            return _db.Trails.Any(p => p.Id == id);
        }

        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return Save();
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }
    }
}