using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RC.CA.Application.MsgBusHandlers;
using RC.CA.Infrastructure.MessageBus.Interfaces;
using static RC.CA.Infrastructure.MessageBus.ServiceBusSubscriptionsManager;

namespace RC.CA.WebApi.Areas.Cdn
{
    /// <summary>
    /// Used for generic testing
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TestController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        
        public TestController(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
        {
            _mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
        }
        /// <summary>
        /// Scope factory to make sure it is actually releasing resources
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task TestMethod()
        {
            //try
            //{
            //    for (int i = 0; i < 1000; i++)
            //    {
            //        using (var scope = _serviceScopeFactory.CreateScope())
            //        {
            //            ProcessCsvImportRequestMessage integrationEvent = new ProcessCsvImportRequestMessage() { ImportId = new Guid("57a2bfbd-4e53-4389-aa08-0e1373c20fe5") };
            //            ////Resolve handler from DI container. This will ensure di references in the constructor will resolve.
            //            var instance = scope.ServiceProvider.GetRequiredService(typeof(ProcessCsvImportRequestMessageHandler));
            //            ////Call handle method
            //            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(typeof(ProcessCsvImportRequestMessage));
            //            await (Task)concreteType.GetMethod("Handle").Invoke(instance, new object[] { integrationEvent });
            //        }
            //    }
            //}
            //catch(Exception ex)
            //{
            //    string m = ex.Message;
            //}
            //GC.Collect();
        }
    }
}
