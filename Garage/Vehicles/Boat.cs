namespace Garage.Vehicles;

public class Boat : Vehicle
{
    public double Length { get; }

    public Boat(string registrationNumber, string color, int numberOfWheels, double length)
        : base(registrationNumber, color, numberOfWheels)
    {
        Length = length;
    }

    public override string ToString() => $"{base.ToString()}, Length: {Length} meters";
}