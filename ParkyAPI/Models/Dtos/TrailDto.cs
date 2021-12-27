using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models.Dtos
{
    public class TrailDto
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public double Distance { get; set; }
        
        public Trail.DifficultyType Difficulty { get; set; }
        
        [Required]
        public int NationalParkId { get; set; }
        
        public NationalPark NationalPark { get; set; }
    }

    public class TrailCreateDto
    {
        [Required]
        public string Name { get; set; }
        
        public double Distance { get; set; }
        
        public Trail.DifficultyType Difficulty { get; set; }
        
        [Required]
        public int NationalParkId { get; set; }
    }
    
    public class TrailUpdateDto
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public double Distance { get; set; }
        
        public Trail.DifficultyType Difficulty { get; set; }
        
        [Required]
        public int NationalParkId { get; set; }
    }
}