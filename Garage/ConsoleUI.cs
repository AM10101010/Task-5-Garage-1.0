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
                case "0": running = false; break;
                default:  Console.WriteLine("Invalid choice. Please try again."); break;
            }
        }
    }

    private static void ShowMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== Garage Menu ===");
        Console.WriteLine("1. List all parked vehicles");
        Console.WriteLine("2. List vehicle types and counts");
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
    private static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
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
}