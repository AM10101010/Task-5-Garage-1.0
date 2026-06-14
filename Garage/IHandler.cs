using Garage.Vehicles;

namespace Garage;

public interface IHandler
{
    int Capacity { get; }
    int Count { get; }

    ParkResult Park(Vehicle vehicle);
    bool Remove(string registrationNumber);
    Vehicle? Find(string registrationNumber);
    IEnumerable<Vehicle> GetAllVehicles();
    IEnumerable<(string VehicleType, int Count)> GetVehicleTypeCounts();
    IEnumerable<Vehicle> Search(string? color = null, int? numberOfWheels = null);
    int Populate(IEnumerable<Vehicle> vehicles);
}

