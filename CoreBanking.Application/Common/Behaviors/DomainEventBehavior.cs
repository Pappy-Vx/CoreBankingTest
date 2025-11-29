using CoreBanking.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Application.Common.Behaviors
{
    public class DomainEventsBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IDomainEventDispatcher _dispatcher;
        private readonly ILogger<DomainEventsBehavior<TRequest, TResponse>> _logger;

        public DomainEventsBehavior(IDomainEventDispatcher dispatcher, ILogger<DomainEventsBehavior<TRequest, TResponse>> logger )
        {
            _dispatcher = dispatcher;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing Domain events for {RequestType}", typeof(TRequest).Name);

            var response = await next();

            //collect and persist domain events
            await _dispatcher.DispatchDomainEventsAsync(cancellationToken);

            return response;
        }
    }
}
