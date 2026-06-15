using System.Text.Json.Serialization;

namespace Garage.Vehicles;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(Car), "car")]
[JsonDerivedType(typeof(Motorcycle), "motorcycle")]
[JsonDerivedType(typeof(Bus), "bus")]
[JsonDerivedType(typeof(Airplane), "airplane")]
[JsonDerivedType(typeof(Boat), "boat")]
    
    public abstract class Vehicle : IVehicle
    {
        public string RegistrationNumber { get; }
        public string Color { get; }
        public int NumberOfWheels { get; }

        protected Vehicle(string registrationNumber, string color, int numberOfWheels)
        {
            RegistrationNumber = registrationNumber;
            Color = color;
            NumberOfWheels = numberOfWheels;
        }

        public override string ToString()
            => $"{GetType().Name}: {RegistrationNumber}, {Color}, {NumberOfWheels} wheels";
    }