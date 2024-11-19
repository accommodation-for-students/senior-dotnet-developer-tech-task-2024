using AFS.TechTask.Common;

namespace AFS.TechTask.Properties.Property
{
    /// <summary>
    /// Represents a single or multi-tenant apartment property for rent.
    /// </summary>
    public class Flat : Property
    {
        public override PropertyType Type => PropertyType.Flat;

        /// <summary>
        /// Initialises a new instance of the <see cref="Flat"/> class.
        /// </summary>
        public Flat(IReadOnlyCollection<Bedroom> bedrooms, IReadOnlyCollection<Photo> photos, Country country)
            : base(bedrooms, photos, country)
        {
        }
    }
}
