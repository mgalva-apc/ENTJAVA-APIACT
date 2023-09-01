namespace SampleWebApiAspNetCore.Entities
{
    public class CharasEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? WeaponType { get; set; }
        public int Level { get; set; }
        public DateTime Obtained { get; set; }
    }
}
