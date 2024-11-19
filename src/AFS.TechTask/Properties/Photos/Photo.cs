namespace AFS.TechTask.Properties
{
    /// <summary>
    /// Class representing a URI to a photograph of a property.
    /// </summary>
    public class Photo
    {
        /// <summary>
        /// String representing a URI to a photograph of a property.
        /// </summary>
        public string Uri { get; init; }

        /// <summary>
        /// Initalises a new instance of the <see cref="Photo"/> class.
        /// </summary>
        /// <param name="uri">A URI to an online photo as a string.</param>
        public Photo(string uri) 
        {
            this.Uri = uri;
        }

        /// <summary>
        /// String representation of this photo's URI.
        /// </summary>
        /// <returns>This photo's URI as a string.</returns>
        public override string ToString() => this.Uri;
    }
}
