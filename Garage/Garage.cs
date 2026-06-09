using System.Collections;
using Garage.Vehicles;

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
}