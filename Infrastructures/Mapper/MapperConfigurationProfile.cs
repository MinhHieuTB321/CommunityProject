using Application.ViewModels.ActivityModel;
using Application.ViewModels.CommunityModels;
using Application.ViewModels.UserModels;
using AutoMapper;
using Domain.Entities;

namespace Infrastructures.Mapper;

public class MapperConfigurationProfile : Profile
{
    public MapperConfigurationProfile()
    {
        CreateMap<User, CreateUserModel>().ReverseMap();
        CreateMap<User, UpdateUserModel>().ReverseMap();
        CreateMap<ViewUserModel, User>().ReverseMap();

        CreateMap<Community, CreateCommunityModel>()
            .ForMember(x => x.Image, options => options.Ignore())
            .ReverseMap();
        CreateMap<Community, UpdateCommunityModel>()
            .ForMember(x => x.Image, options => options.Ignore())
            .ReverseMap();
        CreateMap<ViewCommunityModel, Community>().ReverseMap();

        CreateMap<Activity, CreateActivityModel>()
            .ForMember(x => x.ImageFile, options => options.Ignore())
            .ReverseMap();
        CreateMap<Activity, UpdateActivityModel>()
            .ForMember(x => x.ImageFile, options => options.Ignore())
            .ReverseMap();
        CreateMap<ViewActivityModel, Activity>().ReverseMap();
    }
}