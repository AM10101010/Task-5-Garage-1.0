namespace Garage.Vehicles;

public class Airplane : Vehicle
{
    public double Wingspan { get; }

    public Airplane(string registrationNumber, string color, int numberOfWheels, double wingspan)
        : base(registrationNumber, color, numberOfWheels)
    {
        Wingspan = wingspan;
    }

    public override string ToString() => $"{base.ToString()}, Wingspan: {Wingspan} meters";
}