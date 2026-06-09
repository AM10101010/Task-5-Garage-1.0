namespace Garage.Vehicles;

public class Motorcycle : Vehicle
{
    public int EngineCapacity { get; }

    public Motorcycle(string registrationNumber, string color, int numberOfWheels, int engineCapacity)
        : base(registrationNumber, color, numberOfWheels)
    {
        EngineCapacity = engineCapacity;
    }

    public override string ToString() => $"{base.ToString()}, Engine Capacity: {EngineCapacity} cc";
}