using AFS.TechTask.Common;

namespace AFS.TechTask.Properties.Property
{
    /// <summary>
    /// Base class for all property types
    /// </summary>
    public abstract class Property
    {
        public const int MaxNumberOfPhotos = 14;

        /// <summary>
        /// The type of the property
        /// </summary>
        public abstract PropertyType Type { get; }

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
        /// Base constructor for a class derived from the <see cref="Property"/> class.
        /// </summary>
        protected Property(IReadOnlyCollection<Bedroom> bedrooms, IReadOnlyCollection<Photo> photos, Country country)
        {
            ArgumentNullException.ThrowIfNull(photos);
            ArgumentNullException.ThrowIfNull(bedrooms);
            ArgumentNullException.ThrowIfNull(country);

            if (photos.Count > MaxNumberOfPhotos)
            {
                throw new ArgumentException($"{photos.Count} exceeds the maximum number of photos ({MaxNumberOfPhotos}).");
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
