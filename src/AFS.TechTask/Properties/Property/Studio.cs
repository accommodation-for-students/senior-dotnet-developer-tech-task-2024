using AFS.TechTask.Common;

namespace AFS.TechTask.Properties.Property
{
    /// <summary>
    /// Represents a 1-bedroom studio property for rent.
    /// </summary>
    public class Studio : Property
    {
        public override PropertyType Type => PropertyType.Studio;

        /// <summary>
        /// Initialises a new instance of the <see cref="Studio"/> class.
        /// </summary>
        public Studio(IReadOnlyCollection<Bedroom> bedrooms, IReadOnlyCollection<Photo> photos, Country country)
            : base(bedrooms, photos, country)
        {
        }
    }
}
