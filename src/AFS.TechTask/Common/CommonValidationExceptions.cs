namespace AFS.TechTask.Common
{
    /// <summary>
    /// Represents a validation error when a property's country is not supported.
    /// </summary>
    public class InvalidCountryException : Exception
    {
        public InvalidCountryException(string country) : base($"'{country}' is not a supported country.")
        {
        }
    }

    /// <summary>
    /// Represents a validation error when a bedroom's properties currency value is negative.
    /// </summary>
    public class NegativeCurrencyException : Exception
    {
        public NegativeCurrencyException(string property, int value)
            : base($"{property} price '{value}' cannot be negative.")
        {
        }
    }
}
