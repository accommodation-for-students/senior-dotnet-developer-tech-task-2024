namespace AFS.TechTask.Domain.Properties
{
    /// <summary>
    /// Represents a validation error when a property has an incorrect number of bedrooms.
    /// </summary>
    public class InvalidPropertyTypeException : Exception
    {
        public InvalidPropertyTypeException(string propertyType) 
            : base($"Property type '{propertyType}' is not valid.")
        {
        }
    }

    /// <summary>
    /// Represents a validation error when a property has an incorrect number of photos.
    /// </summary>
    public class InvalidNumberOfPhotosException : Exception
    {
        public InvalidNumberOfPhotosException(int maximum, int actual) 
            : base($"{actual} exceeds the maximum number of photos ({maximum}).")
        {
        }
    }
}
