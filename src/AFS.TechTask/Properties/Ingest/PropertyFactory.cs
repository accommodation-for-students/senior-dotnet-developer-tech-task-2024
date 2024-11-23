using AFS.TechTask.Common;
using AFS.TechTask.Properties.Ingest.Models;
using AFS.TechTask.Properties.Properties;

namespace AFS.TechTask.Properties.Ingest
{
    /// <summary>
    /// A factory for creating validated <see cref="Property"/> instances from <see cref="PropertyResponse"/> instances.
    /// </summary>
    public static class PropertyFactory
    {
        /// <summary>
        /// Factory for transforming <see cref="PropertyResponse"/> records to domain objects derived from <see cref="Property"/>.
        /// </summary>
        /// <param name="property">A property record ingested from an external source.</param>
        /// <returns>Valid <see cref="Property"/> instances.</returns>
        /// <exception cref="InvalidPropertyTypeException"></exception>
        /// <exception cref="InvalidNumberOfPhotosException"></exception>
        /// <exception cref="InvalidNumberOfBedroomsException"></exception>
        /// <exception cref="InvalidRoomSizeException"></exception> 
        /// <exception cref="NegativeCurrencyException"></exception>
        /// <exception cref="InvalidCountryException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static Property Create(PropertyResponse property)
        {
            if (property == null) throw new ArgumentNullException(nameof(PropertyResponse));
            if (property.Bedrooms == null) throw new ArgumentNullException(nameof(property.Bedrooms));
            if (property.Photos == null) throw new ArgumentNullException(nameof(property.Photos));

            if (!Enum.TryParse(typeof(PropertyTypes), property.PropertyType, out object type))
            {
                throw new InvalidPropertyTypeException(property.PropertyType);
            };

            Bedroom[] bedrooms = property.Bedrooms.Select(b => new Bedroom(b)).ToArray();
            Photo[] photos = property.Photos.Select(p => new Photo(p)).ToArray();
            Country country = new Country(property.Country);

            return type switch
            {
                PropertyTypes.Studio => new Studio(bedrooms, photos, country),
                PropertyTypes.Flat => new Flat(bedrooms, photos, country),
                PropertyTypes.House => new House(bedrooms, photos, country),
                _ => throw new InvalidPropertyTypeException($"Property type {type} is not supported.")
            };
        }
    }
}
