using AFS.TechTask.Domain.Common;

namespace AFS.TechTask.Domain.Properties
{
    /// <summary>
    /// Base class for all property types
    /// </summary>
    public abstract class Property
    {
        public const int MaxNumberOfPhotos = 14;

        /// <summary>
        /// Id of the property, once persisted.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// The type of the property
        /// </summary>
        public abstract PropertyTypes Type { get; }

        /// <summary>
        /// The bedrooms for rent in the property
        /// </summary>
        public IReadOnlyCollection<Bedroom> Bedrooms { get; }

        /// <summary>
        /// List of URI's to the photos of the property
        /// </summary>
        public IReadOnlyCollection<Photo> Photos { get; }

        /// <summary>
        /// The country the property is located in.
        /// </summary>
        public Country Country { get; }

        /// <summary>
        /// When the property values were last updated.
        /// </summary>
        public DateTime LastUpdated { get; init; }

        /// <summary>
        /// Base constructor for a class derived from the <see cref="Property"/> class.
        /// </summary>
        protected Property(IReadOnlyCollection<Bedroom> bedrooms, IReadOnlyCollection<Photo> photos, Country country)
        {
            ArgumentNullException.ThrowIfNull(photos);
            ArgumentNullException.ThrowIfNull(bedrooms);
            ArgumentNullException.ThrowIfNull(country);

            if (photos.Count > MaxNumberOfPhotos)
            {
                throw new InvalidNumberOfPhotosException(MaxNumberOfPhotos, photos.Count);
            }

            this.Bedrooms = bedrooms;
            this.Photos = photos;
            this.Country = country;
        }

        /// <summary>
        /// The price of the cheapest available room
        /// </summary>
        public uint? PriceFrom()
        {
            if (this.Bedrooms == null) return null;

            IEnumerable<Bedroom> availableRooms = this.Bedrooms.Where(b => b.Available);

            return availableRooms.Any() ? availableRooms.Min(b => b.Rent) : null;
        }
    }
}
