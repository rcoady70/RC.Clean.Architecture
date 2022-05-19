using NT.CA.Notification.WebApi.MsgBusHandlers;

namespace NT.CA.Notification.WebApi.Services
{
    public interface IEmailProvider
    {
        Task<bool> SendSingleEmailAsync(SendEmailRequestMessage sendEMailMessage);
    }
}
