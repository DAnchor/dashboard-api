using AutoMapper;
using Dashboard.Core.Models;
using Dashboard.Dtos.Payload.User;

namespace Dashboard.Services.Mappings.Profiles.User;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserUpdateDetailsRequestDto, UserModel>()
            .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => ConcatIntoUserName(src.FirstName, src.LastName)))
                .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => ConcatIntoUserEmail(src.FirstName, src.LastName)));

        CreateMap<UserRegistrationRequestDto, UserModel>()
            .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => ConcatIntoUserName(src.FirstName, src.LastName)))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => ConcatIntoUserEmail(src.FirstName, src.LastName)));
    }

    private static string ConcatIntoUserName(string firstName, string lastName)
    {
        return $"{firstName}_{lastName}".ToLower();
    }

    private static string ConcatIntoUserEmail(string firstName, string lastName)
    {
        return $"{firstName}_{lastName}@dashboard.com".ToLower();
    }
}