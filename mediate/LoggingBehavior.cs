using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Smd.Bin.Listing.Common.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private static readonly string RequestName = typeof(TRequest).Name;

    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_logger.IsEnabled(LogLevel.Information))
            return await next().ConfigureAwait(false);

        var sw = Stopwatch.StartNew();

        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["MediatR.Request"] = RequestName
        });

        _logger.LogInformation("Handling {Request}", RequestName);

        try
        {
            var response = await next().ConfigureAwait(false);

            sw.Stop();
            _logger.LogInformation(
                "Handled {Request} in {ElapsedMs} ms",
                RequestName,
                sw.ElapsedMilliseconds);

            return response;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            sw.Stop();
            _logger.LogWarning(
                "Cancelled {Request} after {ElapsedMs} ms",
                RequestName,
                sw.ElapsedMilliseconds);
            throw;
        }
        catch (Exception ex)
        {
            sw.Stop();
            _logger.LogError(
                ex,
                "Error handling {Request} after {ElapsedMs} ms",
                RequestName,
                sw.ElapsedMilliseconds);
            throw;
        }
    }
}
