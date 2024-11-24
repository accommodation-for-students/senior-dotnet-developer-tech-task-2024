using AFS.TechTask.Domain.Common;

namespace AFS.TechTask.Domain.Properties
{
    /// <summary>
    /// Represents a single or multi-tenant apartment property for rent.
    /// </summary>
    public class Flat : Property
    {
        public override PropertyTypes Type => PropertyTypes.Flat;
        public const int MinNumberOfBedrooms = 1;
        public const int MaxNumberOfBedrooms = 14;

        /// <summary>
        /// Initialises a new instance of the <see cref="Flat"/> class.
        /// </summary>
        public Flat(IReadOnlyCollection<Bedroom> bedrooms, IReadOnlyCollection<Photo> photos, Country country)
            : base(bedrooms, photos, country)
        {
            if (bedrooms.Count < MinNumberOfBedrooms || bedrooms.Count > MaxNumberOfBedrooms)
            {
                throw new InvalidNumberOfBedroomsException(this.Type, MaxNumberOfBedrooms, bedrooms.Count);
            }
        }
    }
}
