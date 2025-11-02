using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CoreBankingTest.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) 
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogInformation("Handle command {CommandName} with payloaf {@Request}", requestName, request);

            var timer = System.Diagnostics.Stopwatch.StartNew();
            var response = await next();
            timer.Stop();

            _logger.LogInformation("Command {CommandName} handled in {EllapsedMilliSeconds}ms", requestName, timer.ElapsedMilliseconds);

            return response;
        }
    }
}
