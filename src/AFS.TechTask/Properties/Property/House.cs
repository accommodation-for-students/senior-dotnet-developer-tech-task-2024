using AFS.TechTask.Common;

namespace AFS.TechTask.Properties.Property
{
    /// <summary>
    /// Represents a multi-occupancy house (HMO) for rent.
    /// </summary>
    public class House : Property
    {
        public override PropertyType Type => PropertyType.Studio;

        /// <summary>
        /// Initialises a new instance of the <see cref="House"/> class.
        /// </summary>
        public House(IReadOnlyCollection<Bedroom> bedrooms, IReadOnlyCollection<Photo> photos, Country country)
            : base(bedrooms, photos, country)
        {
        }
    }
}
