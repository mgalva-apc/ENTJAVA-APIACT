using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Models;

namespace SampleWebApiAspNetCore.Repositories
{
    public interface ICharasRepository
    {
        CharasEntity GetSingle(int id);
        void Add(CharasEntity item);
        void Delete(int id);
        CharasEntity Update(int id, CharasEntity item);
        IQueryable<CharasEntity> GetAll(QueryParameters queryParameters);
        ICollection<CharasEntity> GetRandomChara();
        int Count();
        bool Save();
    }
}
