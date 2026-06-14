namespace Garage.Vehicles;

public interface IVehicle
{
    string RegistrationNumber { get; }
    string Color { get; }
    int NumberOfWheels { get; }
}