using AFS.TechTask.Common;

namespace AFS.TechTask.Properties.Property
{
    /// <summary>
    /// Represents a multi-occupancy house (HMO) for rent.
    /// </summary>
    public class House : Property
    {
        public override PropertyType Type => PropertyType.House;
        public const int MaxNumberOfBedrooms = 14;

        /// <summary>
        /// Initialises a new instance of the <see cref="House"/> class.
        /// </summary>
        public House(IReadOnlyCollection<Bedroom> bedrooms, IReadOnlyCollection<Photo> photos, Country country)
            : base(bedrooms, photos, country)
        {
            if (bedrooms.Count < 1 || bedrooms.Count > MaxNumberOfBedrooms)
            {
                throw new ArgumentException($"House properties must have between 1 and {MaxNumberOfBedrooms} bedrooms but was passed {bedrooms.Count} rooms.");
            }
        }
    }
}
