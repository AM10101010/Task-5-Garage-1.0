using System.Text.Json;
using System.Text.Json.Serialization;
using Garage.Vehicles;

namespace Garage;

public static class GarageStorage
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public static string ToJson(IEnumerable<Vehicle> vehicles) =>
        JsonSerializer.Serialize(vehicles.ToList(), Options);

    public static List<Vehicle> FromJson(string json) =>
        JsonSerializer.Deserialize<List<Vehicle>>(json, Options) ?? new();

    public static void Save(string path, IEnumerable<Vehicle> vehicles)
    {
        try
        {
            File.WriteAllText(path, ToJson(vehicles));
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            throw new InvalidOperationException($"Could not save to '{path}': {ex.Message}", ex);
        }
    }

    public static List<Vehicle> Load(string path)
    {
        if (!File.Exists(path))
            return new();                 

        try
        {
            return FromJson(File.ReadAllText(path));
        }
        catch (Exception ex) when (ex is IOException or JsonException)
        {
            throw new InvalidOperationException($"Could not load from '{path}': {ex.Message}", ex);
        }
    }
}