namespace Garage.Vehicles;

public class Bus : Vehicle
{
    public int SeatingCapacity { get; }

    public Bus(string registrationNumber, string color, int numberOfWheels, int seatingCapacity)
        : base(registrationNumber, color, numberOfWheels)
    {
        SeatingCapacity = seatingCapacity;
    }

    public override string ToString() => $"{base.ToString()}, Seating Capacity: {SeatingCapacity}";
}