using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Repositories;

namespace SampleWebApiAspNetCore.Services
{
    public class SeedDataService : ISeedDataService
    {
        public void Initialize(CharasDbContext context)
        {
            context.CharaItems.Add(new CharasEntity() { Level = 80, WeaponType = "Catalyst", Name = "Scaramouche", Obtained = DateTime.Now });
            context.CharaItems.Add(new CharasEntity() { Level = 80, WeaponType = "Bow", Name = "Albedo", Obtained = DateTime.Now });
            context.CharaItems.Add(new CharasEntity() { Level = 90, WeaponType = "Sword", Name = "Kaedehara Kazuha", Obtained = DateTime.Now });
            context.CharaItems.Add(new CharasEntity() { Level = 40, WeaponType = "Bow", Name = "Lyney", Obtained = DateTime.Now });

            context.SaveChanges();
        }
    }
}
