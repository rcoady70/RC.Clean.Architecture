using AutoMapper;
using NT.CA.Notification.WebApi.DTO;
using NT.CA.Notification.WebApi.Models;
using NT.CA.Notification.WebApi.MsgBusHandlers;

namespace NT.CA.Notification.WebApi.MappingProfile;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<SendEmailMessageRequest, SendEmailRequestMessage>().ReverseMap();
        CreateMap<EmailMessageRequest, EmailMessage>().ReverseMap();
    }
}
