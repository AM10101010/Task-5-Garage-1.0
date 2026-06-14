using Garage.Vehicles;

namespace Garage;

public class GarageHandler : IHandler
{
    private readonly Garage<Vehicle> _garage;

    public GarageHandler(int capacity)
    {
        _garage = new Garage<Vehicle>(capacity);
    }

    public int Capacity => _garage.Capacity;
    public int Count => _garage.Count();

    public ParkResult Park(Vehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);
    
        if (_garage.FindByRegistrationNumber(vehicle.RegistrationNumber) is not null)
            return ParkResult.DuplicateRegistration;
        
        return _garage.Add(vehicle) ? ParkResult.Success : ParkResult.GarageFull;
    }
    
    public bool Remove(string registrationNumber) =>
        _garage.Remove(registrationNumber);

    public Vehicle? Find(string registrationNumber) =>
        _garage.FindByRegistrationNumber(registrationNumber);

    public IEnumerable<Vehicle> GetAllVehicles() =>
        _garage.ToList();                 

    public IEnumerable<(string VehicleType, int Count)> GetVehicleTypeCounts() =>
        _garage.CountByVehicleType();

    public IEnumerable<Vehicle> Search(string? color = null, int? numberOfWheels = null) =>
        _garage.Search(color, numberOfWheels);

    public int Populate(IEnumerable<Vehicle> vehicles)
    {
        ArgumentNullException.ThrowIfNull(vehicles);

        int parked = 0;
        foreach (Vehicle vehicle in vehicles)
            if (_garage.Add(vehicle))
                parked++;

        return parked;
    }
}