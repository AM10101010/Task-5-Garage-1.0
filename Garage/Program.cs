using Garage;

ConsoleUI ui = new ConsoleUI(capacity => new GarageHandler(capacity));
ui.Run();