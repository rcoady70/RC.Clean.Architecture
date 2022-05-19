using AutoMapper;
using RC.CA.Application.Dto.Cdn;
using RC.CA.Application.Dto.Club;
using RC.CA.Application.Features.Club.Queries;
using RC.CA.Domain.Entities.Account;
using RC.CA.Domain.Entities.Cdn;
using RC.CA.Domain.Entities.Club;

namespace RC.CA.Application.AutoMapperProfiles;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        //Create maps between models, entities
        CreateMap<Member, MemberListDto>().ReverseMap();
        CreateMap<Member, GetMemberResponseDto>().ReverseMap();
        CreateMap<Member, CreateMemberRequest>().ReverseMap();

        CreateMap<GetMemberResponseDto, CreateMemberRequest>().ReverseMap();

        CreateMap<Experience, CreateExperienceRequest>().ReverseMap();
        CreateMap<Experience, GetMemberExperienceDto>().ReverseMap();

        CreateMap<CreateMemberRequest, GetMemberResponseDto>().ReverseMap();
        CreateMap<CreateExperienceRequest, GetMemberExperienceDto>().ReverseMap();

        CreateMap<UpdateMemberRequest, CreateMemberRequest>().ReverseMap();
        CreateMap<UpdateMemberRequest, Member>().ReverseMap();

        //Cdn
        CreateMap<CdnFiles, CdnListDto > ().ReverseMap();

    }
}

