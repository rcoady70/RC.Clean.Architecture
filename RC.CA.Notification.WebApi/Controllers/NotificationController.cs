using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NT.CA.Notification.WebApi.DTO;
using NT.CA.Notification.WebApi.MsgBusHandlers;
using RC.CA.Infrastructure.MessageBus.Interfaces;

namespace NT.CA.Notification.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;

        public NotificationController(ILogger<NotificationController> logger, IEventBus eventBus, IMapper mapper)
        {
            _logger = logger;
            _eventBus = eventBus;
            _mapper = mapper;
        }
        /// <summary>
        /// Send email notification
        /// </summary>
        /// <param name="sendEmailRequest"></param>
        /// <returns></returns>
        [HttpPost(Name = "SendEmailNotification")]
        public async Task SendEmailNotification(SendEmailMessageRequest sendEmailRequest)
        {
            //if (!ModelState.IsValid) return
            //Publish message to event queue
            var message =  _mapper.Map<SendEmailRequestMessage>(sendEmailRequest);
            _eventBus.Publish(message);
        }
    }
}
