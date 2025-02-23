using AutoMapper;
using MuseumExhibits.Application.DTO;
using MuseumExhibits.Core.Models;

namespace MuseumExhibits.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Exhibit, ExhibitDTO>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category != null
                    ? new CategoryDTO { Id = src.Category.Id, Name = src.Category.Name }
                    : null));

            CreateMap<ExhibitDTO, Exhibit>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category != null ? src.Category.Id : (Guid?)null))
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>()
                .ForMember(dest => dest.Exhibits, opt => opt.Ignore());


        }
    }
}
