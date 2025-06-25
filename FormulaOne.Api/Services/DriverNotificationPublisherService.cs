using FormulaOne.Api.Services.Interfaces;
using FormulaOne.Entities.Contracts;
using MassTransit;

namespace FormulaOne.Api.Services;

public class DriverNotificationPublisherService : IDriverNotificationPublisherService
{
    private readonly ILogger<DriverNotificationPublisherService> _logger;
    private readonly IBus _bus;

    public DriverNotificationPublisherService(
        ILogger<DriverNotificationPublisherService> logger,
        IBus bus)
    {
        _logger = logger;
        _bus = bus;
    }
    
    public async Task SendNotification(Guid driverId, string teamName)
    {
        _logger.LogInformation($"Driver Notification for {driverId} is ready");
        await _bus.Publish(new DriverNotificationRecord(driverId, teamName));
    }
}