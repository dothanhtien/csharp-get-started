using AutoMapper;
using CSharpGetStarted.DTOs;
using CSharpGetStarted.Entities;
using CSharpGetStarted.Extensions;

namespace CSharpGetStarted.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember
                (
                    dest => dest.PhotoUrl, 
                    opts => opts.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url)
                )
                .ForMember
                (
                    dest => dest.Age, 
                    opts => opts.MapFrom(src => src.DateOfBirth.CalculateAge())
                );
            CreateMap<Photo, PhotoDto>();
            CreateMap<UpdateMemberDto, AppUser>();
            CreateMap<Message, MessageDto>()
                .ForMember
                (
                    d => d.SenderPhotoUrl, 
                    o => o.MapFrom(s => s.Sender.Photos.FirstOrDefault(x => x.IsMain).Url)
                )
                .ForMember
                (
                    d => d.RecipientPhotoUrl, 
                    o => o.MapFrom(s => s.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url)
                );
        }
    }
}
