namespace FormulaOne.Service.Email.Interfaces;

public interface IMerchService
{
    void CreateMerch(Guid driverId);
    void RemoveMerch(Guid driverId);
}