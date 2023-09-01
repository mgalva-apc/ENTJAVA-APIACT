using System.ComponentModel.DataAnnotations;

namespace SampleWebApiAspNetCore.Dtos
{
    public class CharaCreateDto
    {
        [Required]
        public string? Name { get; set; }
        public string? WeaponType { get; set; }
        public int Level { get; set; }
        public DateTime Obtained { get; set; }
    }
}
