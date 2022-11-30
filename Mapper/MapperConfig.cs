using AutoMapper;
using VilllaParks.Model;
using VilllaParks.Model.Dto;

namespace VilllaParks.Mapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<VillaPark, VillaParkDto>().ReverseMap();
        }

    }
}
