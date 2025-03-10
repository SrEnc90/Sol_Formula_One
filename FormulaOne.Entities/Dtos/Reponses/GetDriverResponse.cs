﻿namespace FormulaOne.Entities.Dtos.Reponses;

public class GetDriverResponse
{
    public Guid DriverId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int DriverNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
}