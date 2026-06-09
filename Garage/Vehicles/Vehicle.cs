namespace Garage.Vehicles
{
    public abstract class Vehicle
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
}