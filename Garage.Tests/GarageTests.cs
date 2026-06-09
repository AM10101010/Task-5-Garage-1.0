using Garage.Vehicles;

namespace Garage.Tests;

public class GarageTests
{
    [Fact]
    public void Constructor_WithPositiveCapacity_SetsCapacity()
    {
        // Arrange
        var garage = new Garage<Vehicle>(5);
        
        // Assert
        Assert.Equal(5, garage.Capacity);
    }
    
    [Fact]
    public void Enumerating_EmptyGarage_YieldsNoVehicles()
    {
        // Arrange
        var garage = new Garage<Vehicle>(5);
        
        // Act
        Assert.Empty(garage);
    }
    
    [Fact]
    public void Constructor_WithZeroCapacity_ThrowsArgumentOutOfRangeException()
    {
        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Garage<Vehicle>(0));
    }

    [Fact]
    public void Remove_ExistingRegistrationNumber_RemovesVehicle()
    {
        // Arrange
        var garage = new Garage<Vehicle>(5);
        var car = new Car("ABC567", "Red", 4, FuelType.Gasoline);
        
        // Act
        garage.Add(car);
    }
    
    [Fact]
    public void Remove_RegistrationNumber_IsCaseInsensitive()
    {
        // Arrange
        var garage = new Garage<Vehicle>(5);
        garage.Add(new Car("ABC123", "Red", 4, FuelType.Gasoline));
        
        // Act
        bool result = garage.Remove("abc123");
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Remove_RegistrationNumber_RemovesVehicle()
    {
        // Arrange
        var garage = new Garage<Vehicle>(5);
        garage.Add(new Car("ABC345", "Red", 4, FuelType.Gasoline));
        
        // Act
        bool result = garage.Remove("ABC345");
        
        //Assert
        Assert.True(result);
    }

    [Fact]
    public void Remove_FreesSlotForReuse()
    {
        // Arrange
        var garage = new Garage<Vehicle>(5);
        garage.Add(new Car("ABC123", "Red", 4, FuelType.Gasoline));
        garage.Remove("ABC123");
        
        // Assert
        bool result = garage.Add(new Car("DEF321", "Blue", 4, FuelType.Gasoline));
        
        // Act
        Assert.True(result);
    }
}