using AutoMapper;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.JsonPatch;
using MuseumExhibits.Application.DTO;
using MuseumExhibits.Core.Models;
using MuseumExhibits.Core.Filters;

namespace MuseumExhibits.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ExhibitRequest, Exhibit>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Collections, opt => opt.Ignore());

            CreateMap<Exhibit, ExhibitRequest>().ReverseMap();

            CreateMap<Exhibit, ExhibitResponse>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category != null
                    ? new CategoryDTO { Id = src.Category.Id, Name = src.Category.Name }
                    : null))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

            CreateMap<Operation<ExhibitRequest>, Operation<Exhibit>>();

            CreateMap<Exhibit, ExhibitSummaryDTO>()
                .ForMember(dest => dest.MainImageURL, opt => opt.MapFrom(src =>
                    src.Images != null && src.Images.Any(i => i.IsTitleImage)
                        ? src.Images.First(i => i.IsTitleImage).Url
                        : string.Empty));

            CreateMap<ExhibitQueryParameters, ExhibitFilter>()
                .ForMember(dest => dest.Descending,  opt => opt.MapFrom(src => src.Descending ?? true))
                .ForMember(dest => dest.SortBy,      opt => opt.MapFrom(src => src.SortBy ?? "EntryDate"))
                .ForMember(dest => dest.PageNumber,  opt => opt.MapFrom(src => src.PageNumber ?? 1))
                .ForMember(dest => dest.PageSize,    opt => opt.MapFrom(src => Math.Min(src.PageSize ?? 10, 500)));

            CreateMap<JsonPatchDocument<ExhibitRequest>, JsonPatchDocument<Exhibit>>();

            CreateMap<Image, ImageResponse>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>()
                .ForMember(dest => dest.Exhibits, opt => opt.Ignore());

            CreateMap<PostRequest, Post>();
            CreateMap<Post, PostResponse>();
            CreateMap<PostQueryParameters, PostFilter>()
                .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.PageNumber ?? 1))
                .ForMember(dest => dest.PageSize,   opt => opt.MapFrom(src => Math.Min(src.PageSize ?? 10, 200)));

            CreateMap<Collection, CollectionResponse>();
            CreateMap<Collection, CollectionSummaryDTO>()
                .ForMember(dest => dest.ExhibitCount, opt => opt.MapFrom(src => src.Exhibits.Count))
                .ForMember(dest => dest.CoverImageUrl, opt => opt.MapFrom(src =>
                    src.Exhibits
                        .SelectMany(e => e.Images)
                        .Where(i => i.IsTitleImage)
                        .Select(i => i.Url)
                        .FirstOrDefault()
                    ?? src.Exhibits
                        .SelectMany(e => e.Images)
                        .Select(i => i.Url)
                        .FirstOrDefault()));
        }
    }
}
