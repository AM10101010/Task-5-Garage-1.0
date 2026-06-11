using Garage.Vehicles;
namespace Garage;

public class ConsoleUI
{
    public void Run()
    {
        Console.WriteLine("Welcome to the Garage!");

        GarageHandler handler = CreateGarage();
        MenuLoop(handler);

        Console.WriteLine("Goodbye!");
    }

    private GarageHandler CreateGarage()
    {
        int capacity = ReadInt("Enter garage capacity (parking spots): ", min: 1);
        Console.WriteLine($"Created a garage with {capacity} spots.");
        return new GarageHandler(capacity);
    }

    private void MenuLoop(GarageHandler handler)
    {
        bool running = true;
        while (running)
        {
            ShowMenu();
            switch (Console.ReadLine())
            {
                case "1": ListAllVehicles(handler); break;
                case "2": ListVehicleTypeCounts(handler); break;
                case "3": ParkVehicle(handler); break;
                case "0": running = false; break;
                default: Console.WriteLine("Invalid choice. Please try again."); break;
            }
        }
    }

    private static void ShowMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== Garage Menu ===");
        Console.WriteLine("1. List all parked vehicles");
        Console.WriteLine("2. List vehicle types and counts");
        Console.WriteLine("3. Park a vehicle");
        Console.WriteLine("0. Quit");
        Console.Write("Select an option: ");
    }

    private static void ListAllVehicles(GarageHandler handler)
    {
        var vehicles = handler.GetAllVehicles().ToList();
        if (vehicles.Count == 0)
        {
            Console.WriteLine("The garage is empty.");
            return;
        }

        foreach (var vehicle in vehicles)
            Console.WriteLine(vehicle);
    }

    private static void ListVehicleTypeCounts(GarageHandler handler)
    {
        var counts = handler.GetVehicleTypeCounts().ToList();
        if (counts.Count == 0)
        {
            Console.WriteLine("The garage is empty.");
            return;
        }

        foreach (var (type, count) in counts)
            Console.WriteLine($"{type}: {count}");
    }

    // Robust integer input: loops until a valid number in range is entered.
    private static int ReadInt(string prompt,
                                  int min = int.MinValue,
                                  int max = int.MaxValue)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int value) && value >= min && value <= max)
                return value;

            Console.WriteLine($"Please enter a whole number between {min} and {max}.");
        }
    }
    private static string ReadNonEmptyString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
                return input.Trim();

            Console.WriteLine("Input cannot be empty. Please try again!");
        }
    }
    private static double ReadDouble(
        string prompt,
        double min = double.MinValue,
        double max = double.MaxValue)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (double.TryParse(input, out double value) && value >= min && value <= max)
                return value;

            Console.WriteLine($"Please enter a number between {min} and {max}.");
        }
    }
    private static void ParkVehicle(GarageHandler handler)
    {
        Console.WriteLine();
        Console.WriteLine("Choose a vehicle type:");
        Console.WriteLine("1. Car");
        Console.WriteLine("2. Motorcycle");
        Console.WriteLine("3. Bus");
        Console.WriteLine("4. Airplane");
        Console.WriteLine("5. Boat");
        int typeChoice = ReadInt("Type: ", min: 1, max: 5);

        // shared properties
        string registration = ReadNonEmptyString("Registration number: ");
        string color = ReadNonEmptyString("Color: ");
        int wheels = ReadInt("Number of wheels: ", min: 0);

        // build the chosen subclass (type-specific prompt runs only for the matched arm)
        Vehicle vehicle = typeChoice switch
        {
            1 => new Car(registration, color, wheels,
                     ReadInt("Fuel type (1 = Gasoline, 2 = Diesel): ", 1, 2) == 1
                         ? FuelType.Gasoline : FuelType.Diesel),
            2 => new Motorcycle(registration, color, wheels, ReadInt("Engine capacity (cc): ", min: 0)),
            3 => new Bus(registration, color, wheels, ReadInt("Number of seats: ", min: 0)),
            4 => new Airplane(registration, color, wheels, ReadDouble("Wingspan (m): ", min: 0)),
            5 => new Boat(registration, color, wheels, ReadDouble("Length (m): ", min: 0)),
            _ => throw new InvalidOperationException("Unexpected vehicle type.")
        };

        // park and report the outcome
        string message = handler.Park(vehicle) switch
        {
            ParkResult.Success => $"{registration} parked successfully.",
            ParkResult.GarageFull => "Could not park: the garage is full.",
            ParkResult.DuplicateRegistration => $"Could not park: registration '{registration}' is already in use.",
            _ => "Unknown result."
        };
        Console.WriteLine(message);
    }
}