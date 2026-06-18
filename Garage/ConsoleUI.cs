using Garage.Vehicles;
namespace Garage;

public class ConsoleUI : IUI
{
    private readonly Func<int, IHandler> _handlerFactory;
    public ConsoleUI(Func<int, IHandler> handlerFactory)
    {
        _handlerFactory = handlerFactory
            ?? throw new ArgumentNullException(nameof(handlerFactory));
    }

    public void Run()
    {
        Console.WriteLine("Welcome to the Garage!");

        IHandler handler = CreateGarage();

        MenuLoop(handler);

        Console.WriteLine("Goodbye!");
    }

    private IHandler CreateGarage()
    {
        int capacity = ReadInt("Enter garage capacity (parking spots): ", min: 1);
        Console.WriteLine($"Created a garage with {capacity} spots.");
        return _handlerFactory(capacity);
    }

    private void MenuLoop(IHandler handler)
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
                case "4": RemoveVehicle(handler); break;
                case "5": FindVehicle(handler); break;
                case "6": SearchVehicles(handler); break;
                case "7": PopulateWithSamples(handler); break;
                case "8": SaveGarage(handler); break;
                case "9": LoadGarage(handler); break;
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
        Console.WriteLine("4. Remove a vehicle");
        Console.WriteLine("5. Find a vehicle by registration");
        Console.WriteLine("6. Search vehicles");
        Console.WriteLine("7. Populate with sample vehicles");
        Console.WriteLine("8. Save garage to file");
        Console.WriteLine("9. Load garage from file");
        Console.WriteLine("0. Quit");
        Console.Write("Select an option: ");
    }

    private static void ListAllVehicles(IHandler handler)
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

    private static void ListVehicleTypeCounts(IHandler handler)
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
    private static void ParkVehicle(IHandler handler)
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
    private static bool ReadYesNo(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine()?.Trim().ToLowerInvariant();

            if (input is "y" or "yes") return true;
            if (input is "n" or "no") return false;

            Console.WriteLine("Please answer y or n.");
        }
    }
    private static IEnumerable<Vehicle> CreateSampleVehicles() => new Vehicle[]
    {
        new Car("ABC123", "Red", 4, FuelType.Gasoline),
        new Motorcycle("MC1000", "Black", 2, 600),
        new Bus("BUS500", "Yellow", 6, 50),
        new Airplane("AIR747", "White", 3, 60.0),
        new Boat("SEA001", "Blue", 0, 8.5),
    };

    private static void PopulateWithSamples(IHandler handler)
    {
        if (!ReadYesNo("Populate with sample vehicles? (y/n): "))
            return;

        int parked = handler.Populate(CreateSampleVehicles());
        Console.WriteLine($"Populated the garage with {parked} sample vehicle(s).");
    }

    private static string? ReadOptionalString(string prompt)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) ? null : input.Trim();
    }

    private static int? ReadOptionalInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                return null;                       // blank → no filter

            if (int.TryParse(input, out int value))
                return value;

            Console.WriteLine("Please enter a whole number, or leave blank to skip.");
        }
    }
    private static void FindVehicle(IHandler handler)
    {
        string registration = ReadNonEmptyString("Registration number to find: ");
        Vehicle? vehicle = handler.Find(registration);

        Console.WriteLine(vehicle is not null
            ? vehicle.ToString()
            : $"No vehicle with registration '{registration}' was found.");
    }

    private static void SearchVehicles(IHandler handler)
    {
        Console.WriteLine("Leave a field blank to skip that filter.");
        string? color = ReadOptionalString("Color: ");
        int? wheels = ReadOptionalInt("Number of wheels: ");
        string? type = ReadOptionalString("Vehicle type (Car, Motorcycle, Bus, Airplane, Boat): ");

        IEnumerable<Vehicle> results = handler.Search(color, wheels);
        if (type is not null)
            results = results.Where(v =>
                string.Equals(v.GetType().Name, type, StringComparison.OrdinalIgnoreCase));

        var list = results.ToList();
        if (list.Count == 0)
        {
            Console.WriteLine("No matching vehicles found.");
            return;
        }

        foreach (var vehicle in list)
            Console.WriteLine(vehicle);
    }
    private static void RemoveVehicle(IHandler handler)
    {
        string registration = ReadNonEmptyString("Registration number to remove: ");
        bool removed = handler.Remove(registration);

        Console.WriteLine(removed
            ? $"{registration} was removed."
            : $"No vehicle with registration '{registration}' was found.");
    }
    private const string SaveFilePath = "garage.json";

    private static void SaveGarage(IHandler handler)
    {
        try
        {
            handler.SaveToFile(SaveFilePath);
            Console.WriteLine($"Garage saved to {SaveFilePath}.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static void LoadGarage(IHandler handler)
    {
        try
        {
            int loaded = handler.LoadFromFile(SaveFilePath);
            Console.WriteLine($"Loaded {loaded} vehicle(s) from {SaveFilePath}.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}