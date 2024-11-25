namespace AFS.TechTask.Domain.Properties
{
    /// <summary>
    /// Represents a validation error when a property has an invalid number of bedrooms.
    /// </summary>
    public class InvalidNumberOfBedroomsException : Exception
    {
        public InvalidNumberOfBedroomsException(PropertyTypes type, int maximum, int actual)
            : base($"{type} properties must have at least 1 room up to a maximum of {maximum} rooms but was passed {actual} rooms.")
        {
        }
    }

    /// <summary>
    /// Represents a validation error when a bedroom size description is not supported.
    /// </summary>
    public class InvalidRoomSizeException : Exception
    {
        public InvalidRoomSizeException(string roomSize) : base($"Room size '{roomSize}' is not valid.")
        {
        }
    }

    /// <summary>
    /// Represents a validation error when a bed size description is not supported.
    /// </summary>
    public class InvalidBedSizeException : Exception
    {
        public InvalidBedSizeException(string bedSize) : base($"Bed size '{bedSize}' is not valid.")
        {
        }
    }
}
