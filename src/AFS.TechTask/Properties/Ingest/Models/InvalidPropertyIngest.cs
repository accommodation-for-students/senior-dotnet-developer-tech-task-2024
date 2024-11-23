namespace AFS.TechTask.Properties.Ingest.Models
{
    /// <summary>
    /// A record of an invalid property that failed to be ingested from an external source.
    /// </summary>
    /// <param name="property">An ingested <see cref="PropertyResponse"/> that failed validation.</param>
    /// <param name="exception">The exception generated during validation.</param>
    public class InvalidPropertyIngest
    {
        public PropertyResponse Property { get; }
        public string ValidationError { get; }

        public InvalidPropertyIngest(PropertyResponse property, Exception exception)
        {
            this.Property = property;
            this.ValidationError = exception?.Message;
        }
    };
}
