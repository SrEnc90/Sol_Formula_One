using FormulaOne.Entities.Contracts;
using MassTransit;

namespace FIA.Api.Services;

public class DriverNotificationConsumer : IConsumer<DriverNotificationRecord>
{
    private readonly ILogger<DriverNotificationConsumer> _logger;

    public DriverNotificationConsumer(ILogger<DriverNotificationConsumer> logger)
    {
        _logger = logger;
    }
    
    public Task Consume(ConsumeContext<DriverNotificationRecord> context)
    {
        _logger.LogInformation("FIA Log: " + context.Message.DriverId + "Name:" + context.Message.DriverName );
        return Task.CompletedTask;
    }
}