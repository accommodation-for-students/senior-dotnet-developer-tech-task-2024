using AFS.TechTask.Domain.Common;

namespace AFS.TechTask.Domain.Properties
{
    /// <summary>
    /// Represents a 1-bedroom studio property for rent.
    /// </summary>
    public class Studio : Property
    {
        public override PropertyTypes Type => PropertyTypes.Studio;
        public const int ExactNumberOfBedrooms = 1;

        /// <summary>
        /// Initialises a new instance of the <see cref="Studio"/> class.
        /// </summary>
        public Studio(IReadOnlyCollection<Bedroom> bedrooms, IReadOnlyCollection<Photo> photos, Country country)
            : base(bedrooms, photos, country)
        {
            if (bedrooms.Count != ExactNumberOfBedrooms)
            {
                throw new InvalidNumberOfBedroomsException(this.Type, ExactNumberOfBedrooms, bedrooms.Count);
            }
        }
    }
}
