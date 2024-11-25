namespace AFS.TechTask.Domain.Common
{
    /// <summary>
    /// Represents a validation error when a country is not supported.
    /// </summary>
    public class InvalidCountryException : Exception
    {
        public InvalidCountryException(string country) : base($"'{country}' is not a supported country.")
        {
        }
    }

    /// <summary>
    /// Represents a validation error when a currency value is negative.
    /// </summary>
    public class NegativeCurrencyException : Exception
    {
        public NegativeCurrencyException(string property, int value)
            : base($"{property} price '{value}' cannot be negative.")
        {
        }
    }
}
