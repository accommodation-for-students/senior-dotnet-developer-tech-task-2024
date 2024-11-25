using AFS.TechTask.Domain.Common;

namespace AFS.TechTask.Domain.Properties
{
    /// <summary>
    /// Represents a multi-occupancy house (HMO) for rent.
    /// </summary>
    public class House : Property
    {
        public override PropertyTypes Type => PropertyTypes.House;
        public const int MinNumberOfBedrooms = 1;
        public const int MaxNumberOfBedrooms = 14;

        /// <summary>
        /// Initialises a new instance of the <see cref="House"/> class.
        /// </summary>
        public House(IReadOnlyCollection<Bedroom> bedrooms, IReadOnlyCollection<Photo> photos, Country country)
            : base(bedrooms, photos, country)
        {
            if (bedrooms.Count < MinNumberOfBedrooms || bedrooms.Count > MaxNumberOfBedrooms)
            {
                throw new InvalidNumberOfBedroomsException(this.Type, MaxNumberOfBedrooms, bedrooms.Count);
            }
        }
    }
}
