using FormulaOne.Service.Repositories.Interfaces;

namespace FormulaOne.Service.Repositories;

public class MaintenanceService : IMaintenanceService
{
    public void SyncRecords()
    {
        Console.WriteLine($"The sync has started");
    }
}