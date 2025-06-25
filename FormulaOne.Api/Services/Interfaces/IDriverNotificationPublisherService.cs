namespace FormulaOne.Api.Services.Interfaces;

public interface IDriverNotificationPublisherService
{
    Task SendNotification(Guid driverId, string teamName);
}