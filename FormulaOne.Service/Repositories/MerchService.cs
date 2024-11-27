using FormulaOne.Service.Email.Interfaces;

namespace FormulaOne.Service.Repositories;

public class MerchService: IMerchService
{
    public void CreateMerch(Guid driverId)
    {
        Console.WriteLine($"This will create Merch for the driver {driverId}");
    }

    public void RemoveMerch(Guid driverId)
    {
        Console.WriteLine($"This will remove Merch for the driver {driverId}");
    }
}