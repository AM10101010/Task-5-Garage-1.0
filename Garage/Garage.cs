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

        if (CheckRegistrationNumber(vehicle.RegistrationNumber))
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
        if (string.IsNullOrWhiteSpace(registrationNumber))
            return false;

        for (int i = 0; i < _vehicles.Length; i++)
        {
            T? vehicle = _vehicles[i];
            if (vehicle is not null && string.Equals(vehicle.RegistrationNumber, registrationNumber,
                    StringComparison.OrdinalIgnoreCase))
            {
                _vehicles[i] = null;
                return true;
            }
        }

        return false;
    }

    private bool CheckRegistrationNumber(string registrationNumber) =>
        _vehicles.Any(v => v is not null &&
                           string.Equals(v.RegistrationNumber, registrationNumber, StringComparison.OrdinalIgnoreCase));
}

    