using Garage.Vehicles;

namespace Garage.Tests;

public class GarageTests
{
    [Fact]
    public void Constructor_WithPositiveCapacity_SetsCapacity()
    {
        var garage = new Garage<Vehicle>(5);
        Assert.Equal(5, garage.Capacity);
    }
    
    [Fact]
    public void Enumerating_EmptyGarage_YieldsNoVehicles()
    {
        var garage = new Garage<Vehicle>(5);
        Assert.Empty(garage);
    }
    
    [Fact]
    public void Constructor_WithZeroCapacity_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Garage<Vehicle>(0));
    }
}