using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Models;
using System.Linq.Dynamic.Core;

namespace SampleWebApiAspNetCore.Repositories
{
    public class CharasSqlRepository : ICharasRepository
    {
        private readonly CharasDbContext _charasDbContext;

        public CharasSqlRepository(CharasDbContext charasDbContext)
        {
            _charasDbContext = charasDbContext;
        }

        public CharasEntity GetSingle(int id)
        {
            return _charasDbContext.CharaItems.FirstOrDefault(x => x.Id == id);
        }

        public void Add(CharasEntity item)
        {
            _charasDbContext.CharaItems.Add(item);
        }

        public void Delete(int id)
        {
            CharasEntity charaItem = GetSingle(id);
            _charasDbContext.CharaItems.Remove(charaItem);
        }

        public CharasEntity Update(int id, CharasEntity item)
        {
            _charasDbContext.CharaItems.Update(item);
            return item;
        }

        public IQueryable<CharasEntity> GetAll(QueryParameters queryParameters)
        {
            IQueryable<CharasEntity> _allItems = _charasDbContext.CharaItems.OrderBy(queryParameters.OrderBy,
              queryParameters.IsDescending());

            if (queryParameters.HasQuery())
            {
                _allItems = _allItems
                    .Where(x => x.Level.ToString().Contains(queryParameters.Query.ToLowerInvariant())
                    || x.Name.ToLowerInvariant().Contains(queryParameters.Query.ToLowerInvariant()));
            }

            return _allItems
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
        }

        public int Count()
        {
            return _charasDbContext.CharaItems.Count();
        }

        public bool Save()
        {
            return (_charasDbContext.SaveChanges() >= 0);
        }

        public ICollection<CharasEntity> GetRandomChara()
        {
            List<CharasEntity> toReturn = new List<CharasEntity>();

            toReturn.Add(GetRandomChara("Sword"));
            toReturn.Add(GetRandomChara("Catalyst"));
            toReturn.Add(GetRandomChara("Bow"));

            return toReturn;
        }

        private CharasEntity GetRandomChara(string type)
        {
            return _charasDbContext.CharaItems
                .Where(x => x.WeaponType == type)
                .OrderBy(o => Guid.NewGuid())
                .FirstOrDefault();
        }
    }
}
