namespace SampleWebApiAspNetCore.Dtos
{
    public class CharasDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? WeaponType { get; set; }
        public int Level { get; set; }
        public DateTime Obtained { get; set; }
    }
}
