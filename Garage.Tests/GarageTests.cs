using Garage.Vehicles;

namespace Garage.Tests;

public class GarageTests
{
    [Fact]
    public void Constructor_WithPositiveCapacity_SetsCapacity()
    {
        // Act
        var garage = new Garage<Vehicle>(5);

        // Assert
        Assert.Equal(5, garage.Capacity);
    }

    [Fact]
    public void Enumerating_EmptyGarage_YieldsNoVehicles()
    {
        // Arrange
        var garage = new Garage<Vehicle>(5);

        // Act & Assert
        Assert.Empty(garage);
    }

    [Fact]
    public void Constructor_WithZeroCapacity_ThrowsArgumentOutOfRangeException()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Garage<Vehicle>(0));
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

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Remove_FreesSlotForReuse()
    {
        // Arrange
        var garage = new Garage<Vehicle>(1);
        garage.Add(new Car("ABC123", "Red", 4, FuelType.Gasoline));
        garage.Remove("ABC123");

        // Act
        bool result = garage.Add(new Car("DEF321", "Blue", 4, FuelType.Gasoline));

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Add_WhenSpaceAvailable_ParksVehicleAndReturnsTrue()
    {
        // Arrange
        var garage = new Garage<Vehicle>(2);
        var car = new Car("ABC123", "Red", 4, FuelType.Gasoline);

        // Act
        bool result = garage.Add(car);

        // Assert
        Assert.True(result);
        Assert.Contains(car, garage);
    }

    [Fact]
    public void Add_WhenGarageIsFull_ReturnsFalse()
    {
        // Arrange
        var garage = new Garage<Vehicle>(1);
        garage.Add(new Car("ABC123", "Red", 4, FuelType.Gasoline));

        // Act
        bool result = garage.Add(new Car("XYZ789", "Blue", 4, FuelType.Diesel));

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Add_WithDuplicateRegistrationNumber_IgnoringCase_ReturnsFalse()
    {
        // Arrange
        var garage = new Garage<Vehicle>(5);
        garage.Add(new Car("ABC123", "Red", 4, FuelType.Gasoline));

        // Act
        bool result = garage.Add(new Car("abc123", "Blue", 4, FuelType.Diesel));

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Add_NullVehicle_ThrowsArgumentNullException()
    {
        // Arrange
        var garage = new Garage<Vehicle>(5);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => garage.Add(null!));
    }
}