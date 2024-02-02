using AutoMapper;
using EventifyCommon.Models;
using EventifyWebApi.DTOs;

namespace EventifyWebApi.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() {
            CreateMap<Event, EventDto>()
              .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
              .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency.Name))
              .ForMember(dest => dest.Format, opt => opt.MapFrom(src => src.Format.Name))
              .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language.Name))
              .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images.Select(img => img.ImageUrl)))
              .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(tag => tag.Name)))
              .ReverseMap();

            CreateMap<CreateEventRequestDto, Event>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(tagName => new Tag { Name = tagName })))
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ReverseMap();

            CreateMap<UpdateEventRequestDto, Event>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(tagName => new Tag { Name = tagName })))
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ReverseMap();


            /* CreateMap<CurrencyDto, Currency>()
             .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
             .ForMember(dest => dest.Events, opt => opt.Ignore()); // Ignore Events property or handle mapping separately

             CreateMap<Currency, CurrencyDto>()
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));*/
            CreateMap<Format, FormatDto>().ReverseMap();
            CreateMap<EventAgend, EventAgendDto>().ReverseMap();
            CreateMap<Language, LanguageDto>().ReverseMap();
            CreateMap<Currency, CurrencyDto>().ReverseMap();    
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Tag, TagRequestDto>().ReverseMap();
            CreateMap<Tag, TagDto>().ReverseMap();
            CreateMap<Like, LikeDto>().ReverseMap();
            CreateMap<Like, CreateLikeRequestDto>().ReverseMap();


            CreateMap<ApplicationUser, UserDto>()
            .ForMember(dest => dest.FollowersCount, opt => opt.MapFrom(src => src.FollowerFollows.Count()))
            .ReverseMap();



            CreateMap<ClosedAccountReason, ClosedAccountReasonDto>().ReverseMap();
            CreateMap<ReportEventReason, ReportEventReasonDto>().ReverseMap();

            CreateMap<Follow, FollowDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FollowedUser.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.FollowedUser.LastName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.FollowedUser.ImageUrl))
                .ForMember(dest => dest.FollowersCount, opt => opt.Ignore()); // Ignore the property for now


            CreateMap<ApplicationUser, FollowDto>()
               .ForMember(dest => dest.FollowedUserId, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName ?? ""))
               .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName ?? ""))
               .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl ?? ""))
               .ForMember(dest => dest.FollowersCount, opt => opt.MapFrom(src => src.FollowerFollows.Count))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ReverseMap();

            CreateMap<Follow, CreateFollowRequestDto>().ReverseMap();
            CreateMap<ClosedAccount, CreateClosedAccountRequestDto>().ReverseMap();
            CreateMap<ClosedAccount, ClosedAccountDto>().ForMember(dest => dest.ClosedAccountReason, opt => opt.MapFrom(src => src.ClosedAccountReason.Name)).ReverseMap();
            CreateMap<ReportEvent, CreateReportEventRequestDto>().ReverseMap();
            CreateMap<ReportEvent, ReportEventDto>()
            .ForMember(dest => dest.ReportEventReason, opt => opt.MapFrom(src => src.ReportEventReason.Name))
            .ForMember(dest => dest.Event, opt => opt.MapFrom(src => src.Event))
            .ReverseMap();





            CreateMap<Format, FormatDto>().ReverseMap();    
            CreateMap<Language, LanguageDto>().ReverseMap();
        }
    }
}
