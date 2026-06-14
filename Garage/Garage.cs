using System.Collections;
using Garage.Vehicles;
using System.Linq;
namespace Garage;

public class Garage<T> : IEnumerable<T> where T : Vehicle
{
    private readonly T?[] _vehicles;

    public Garage(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity),
                "Capacity must be greater than zero.");

        _vehicles = new T?[capacity];
    }

    public int Capacity => _vehicles.Length;

    public IEnumerator<T> GetEnumerator()
    {
        foreach (T? vehicle in _vehicles)
        {
            if (vehicle is not null)
                yield return vehicle;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool Add(T vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        if (IndexOf(vehicle.RegistrationNumber) >= 0)
            return false;

        for (int i = 0; i < _vehicles.Length; i++)
        {
            if (_vehicles[i] is null)
            {
                _vehicles[i] = vehicle;
                return true;
            }
        }
        return false;
    }

    public bool Remove(string registrationNumber)
    {
        int index = IndexOf(registrationNumber);
        if (index < 0)
            return false;
        _vehicles[index] = null;
        return true;
    }

    private int IndexOf(string registrationNumber)
    {
        if (string.IsNullOrWhiteSpace(registrationNumber))
            return -1;

        for (int i = 0; i < _vehicles.Length; i++)
        {
            T? vehicle = _vehicles[i];
            if (vehicle is not null &&
                string.Equals(vehicle.RegistrationNumber, registrationNumber,
                    StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }

        return -1;
    }

    public T? FindByRegistrationNumber(string registrationNumber)
    {
        int index = IndexOf(registrationNumber);
        return index >= 0 ? _vehicles[index] : null;
    }

    public IEnumerable<(string VehicleType, int Count)> CountByVehicleType()
    {
        return this
            .GroupBy(vehicle => vehicle.GetType().Name)
            .Select(group => (VehicleType: group.Key, Count: group.Count()))
            .OrderBy(item => item.VehicleType);
    }

    public IEnumerable<T> Search(string? color = null, int? numberOfWheels = null)
    {
        return this.Where(vehicle =>
            (color is null || string.Equals(vehicle.Color, color, StringComparison.OrdinalIgnoreCase)) &&
            (numberOfWheels is null || vehicle.NumberOfWheels == numberOfWheels));
    }
}