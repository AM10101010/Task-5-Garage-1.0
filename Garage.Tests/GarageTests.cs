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
    [Fact]
    public void FindByRegistrationNumber_ExistingVehicle_ReturnsThatVehicle()
    {
        // Arrange
        var garage = new Garage<Vehicle>(5);
        var car = new Car("ABC123", "Red", 4, FuelType.Gasoline);
        garage.Add(car);

        // Act
        var found = garage.FindByRegistrationNumber("ABC123");

        // Assert
        Assert.Same(car, found);
    }

    [Fact]
    public void FindByRegistrationNumber_IsCaseInsensitive()
    {
        // Arrange
        var garage = new Garage<Vehicle>(5);
        var car = new Car("ABC123", "Red", 4, FuelType.Gasoline);
        garage.Add(car);

        // Act
        var found = garage.FindByRegistrationNumber("aBc123");

        // Assert
        Assert.Same(car, found);
    }

    [Fact]
    public void FindByRegistrationNumber_NonExisting_ReturnsNull()
    {
        // Arrange
        var garage = new Garage<Vehicle>(5);
        garage.Add(new Car("ABC123", "Red", 4, FuelType.Gasoline));

        // Act
        var found = garage.FindByRegistrationNumber("ZZZ999");

        // Assert
        Assert.Null(found);
    }

    [Fact]
    public void FindByRegistrationNumber_NullOrEmpty_ReturnsNull()
    {
        // Arrange
        var garage = new Garage<Vehicle>(5);
        garage.Add(new Car("ABC123", "Red", 4, FuelType.Gasoline));

        // Act & Assert
        Assert.Null(garage.FindByRegistrationNumber(""));
        Assert.Null(garage.FindByRegistrationNumber(null!));
    }
    
    [Fact]
    public void CountByVehicleType_GroupsAndCountsByConcreteType()
    {
        // Arrange
        var garage = new Garage<Vehicle>(5);
        garage.Add(new Car("ABC123", "Red", 4, FuelType.Gasoline));
        garage.Add(new Car("DEF456", "Blue", 4, FuelType.Diesel));
        garage.Add(new Motorcycle("MC0001", "Black", 2, 600));

        // Act
        var counts = garage.CountByVehicleType()
            .ToDictionary(x => x.VehicleType, x => x.Count);

        // Assert
        Assert.Equal(2, counts["Car"]);
        Assert.Equal(1, counts["Motorcycle"]);
    }

    [Fact]
    public void CountByVehicleType_EmptyGarage_ReturnsEmpty()
    {
        // Arrange
        var garage = new Garage<Vehicle>(5);

        // Act & Assert
        Assert.Empty(garage.CountByVehicleType());
    }
    [Fact]
    public void Search_ByColor_ReturnsMatchingVehicles()
    {
        // Arrange
        var garage = new Garage<Vehicle>(5);
        var red1 = new Car("ABC123", "Red", 4, FuelType.Gasoline);
        var red2 = new Motorcycle("MC0001", "Red", 2, 600);
        garage.Add(red1);
        garage.Add(red2);
        garage.Add(new Car("DEF456", "Blue", 4, FuelType.Diesel));

        // Act
        var results = garage.Search(color: "Red").ToList();

        // Assert
        Assert.Equal(2, results.Count);
        Assert.Contains(red1, results);
        Assert.Contains(red2, results);
    }
    [Fact]
    public void Search_ByColorAndWheels_ReturnsOnlyVehiclesMatchingBoth()
    {
        // Arrange
        var garage = new Garage<Vehicle>(5);
        var match = new Car("ABC123", "Black", 4, FuelType.Gasoline);
        garage.Add(match);
        garage.Add(new Motorcycle("MC0001", "Black", 2, 600));   // black, but 2 wheels
        garage.Add(new Car("DEF456", "Red", 4, FuelType.Diesel)); // 4 wheels, but red

        // Act
        var results = garage.Search(color: "Black", numberOfWheels: 4).ToList();

        // Assert
        Assert.Single(results);
        Assert.Contains(match, results);
    }
    [Fact]
    public void Search_NoCriteria_ReturnsAllVehicles()
    {
        // Arrange
        var garage = new Garage<Vehicle>(5);
        garage.Add(new Car("ABC123", "Red", 4, FuelType.Gasoline));
        garage.Add(new Motorcycle("MC0001", "Black", 2, 600));

        // Act
        var results = garage.Search().ToList();

        // Assert
        Assert.Equal(2, results.Count);
    }
    [Fact]
    public void Search_ColorIsCaseInsensitive()
    {
        // Arrange
        var garage = new Garage<Vehicle>(5);
        garage.Add(new Car("ABC123", "Red", 4, FuelType.Gasoline));

        // Act
        var results = garage.Search(color: "red").ToList();

        // Assert
        Assert.Single(results);
    }
    [Fact]
    public void Park_DuplicateRegistration_ReturnsDuplicateResult()
    {
        var handler = new GarageHandler(5);
        handler.Park(new Car("ABC123", "Red", 4, FuelType.Gasoline));

        var result = handler.Park(new Car("abc123", "Blue", 4, FuelType.Diesel));

        Assert.Equal(ParkResult.DuplicateRegistration, result);
    }
    [Fact]
    public void Clear_RemovesAllVehicles()
    {
        // Arrange
        var garage = new Garage<Vehicle>(5);
        garage.Add(new Car("ABC123", "Red", 4, FuelType.Gasoline));
        garage.Add(new Car("DEF456", "Blue", 4, FuelType.Diesel));

        // Act
        garage.Clear();

        // Assert
        Assert.Empty(garage);
    }
    [Fact]
    public void ToJson_ThenFromJson_PreservesTypesAndProperties()
    {
        // Arrange
        var original = new List<Vehicle>
        {
            new Car("ABC123", "Red", 4, FuelType.Diesel),
            new Boat("BOAT01", "White", 0, 6.5),
        };

        // Act
        string json = GarageStorage.ToJson(original);
        List<Vehicle> restored = GarageStorage.FromJson(json);

        // Assert
        Assert.Equal(2, restored.Count);

        var car = Assert.IsType<Car>(restored[0]);     // type preserved
        Assert.Equal("ABC123", car.RegistrationNumber); // base property
        Assert.Equal(FuelType.Diesel, car.FuelType);    // subclass-specific property

        var boat = Assert.IsType<Boat>(restored[1]);
        Assert.Equal(6.5, boat.Length);
    }
}