using AFS.TechTask.Domain.Common;
using AFS.TechTask.Domain.Properties;
using FluentAssertions;

namespace AFS.TechTask.UnitTests.Domain.Properties
{
    public class BedroomTests
    {
        [Theory]
        [InlineData("single")]
        [InlineData("sInGlE")]
        [InlineData("SINGLE")]
        [InlineData("double")]
        [InlineData("dOuBlE")]
        [InlineData("DOUBLE")]
        [InlineData("king size")]
        [InlineData("kInG sIzE")]
        [InlineData("KING SIZE")]
        public void NewBedroom_ValidBedSize_CaseInsensitive_ConstructsOK(string bedSize)
        {
            // Act
            Bedroom result = new (
                available: true,
                roomSize: "large",
                bedSize: bedSize,
                rent: 425,
                deposit: 750);

            // Assert
            result.BedSize.Should().Be(bedSize.ToLower());
        }

        [Theory]
        [InlineData("small")]
        [InlineData("medium")]
        [InlineData("large")]
        [InlineData("kingize")]
        [InlineData("sgl")]
        [InlineData("db")]
        [InlineData("ks")]
        public void NewBedroom_InvalidBedSize_Throws(string bedSize)
        {
            // Act
            Action action = () => new Bedroom(
                available: true, 
                roomSize: "large",
                bedSize: bedSize,
                rent: 425,
                deposit: 750);

            // Assert
            action.Should().Throw<InvalidBedSizeException>();
        }

        [Theory]
        [InlineData("small")]
        [InlineData("sMaLl")]
        [InlineData("SMALL")]
        [InlineData("medium")]
        [InlineData("mEdIuM")]
        [InlineData("MEDIUM")]
        [InlineData("large")]
        [InlineData("lArGe")]
        [InlineData("LARGE")]
        public void NewBedroom_ValidRoomSize_CaseInsensitive_ConstructsOK(string roomSize)
        {
            // Act
            Bedroom result = new(
                available: true,
                roomSize: roomSize,
                bedSize: "single",
                rent: 425,
                deposit: 750);

            // Assert
            result.RoomSize.Should().Be(roomSize.ToLower());
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        public void NewBedroom_InvalidRent_Throws(int rent)
        {
            // Act
            Action action = () => new Bedroom(
                available: true,
                roomSize: "large",
                bedSize: "single",
                rent: rent,
                deposit: 750);

            // Assert
            action.Should().Throw<NegativeCurrencyException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(int.MaxValue)]
        public void NewBedroom_ValidRent_ConstructsOK(int rent)
        {
            // Act
            Bedroom result = new(
                available: true,
                roomSize: "large",
                bedSize: "single",
                rent: rent,
                deposit: 750);

            // Assert
            Convert.ToInt32(result.Rent).Should().Be(rent);
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        public void NewBedroom_InvalidDeposit_Throws(int deposit)
        {
            // Act
            Action action = () => new Bedroom(
                available: true,
                roomSize: "large",
                bedSize: "single",
                rent: 425,
                deposit: deposit);

            // Assert
            action.Should().Throw<NegativeCurrencyException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(int.MaxValue)]
        public void NewBedroom_ValidDeposit_ConstructsOK(int deposit)
        {
            // Act
            Bedroom result = new(
                available: true,
                roomSize: "large",
                bedSize: "single",
                rent: 425,
                deposit: deposit);

            // Assert
            Convert.ToInt32(result.Deposit).Should().Be(deposit);
        }
    }
}
