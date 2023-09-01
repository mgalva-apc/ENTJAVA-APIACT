using AutoMapper;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.MappingProfiles
{
    public class CharasMappings : Profile
    {
        public CharasMappings()
        {
            CreateMap<CharasEntity, CharasDto>().ReverseMap();
            CreateMap<CharasEntity, CharasUpdateDto>().ReverseMap();
            CreateMap<CharasEntity, CharaCreateDto>().ReverseMap();
        }
    }
}
