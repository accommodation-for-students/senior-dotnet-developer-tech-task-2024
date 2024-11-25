using AFS.TechTask.Application.Properties.Ingest;
using AFS.TechTask.Data.Properties;
using AFS.TechTask.Domain.Common;
using AFS.TechTask.Domain.Properties;

namespace AFS.TechTask.Application.Properties
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

            Bedroom[] bedrooms = property.Bedrooms.Select(b => new Bedroom(b.Available, b.RoomSize, b.BedSize, b.Rent, b.Deposit)).ToArray();
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

        /// <summary>
        /// Factory for transforming property-related models from the datasource to domain objects 
        /// derived from <see cref="Property"/>.
        /// </summary>
        /// <param name="propertyModel">A property record from the datasource.</param>
        /// <param name="bedroomModels">A collection of bedroom data models from the datasource.</param>
        /// <param name="photoModels">A collection of photo data models from the datasource.</param>
        /// <returns>Valid <see cref="Property"/> instances.</returns>
        /// <exception cref="InvalidPropertyTypeException"></exception>
        /// <exception cref="InvalidNumberOfPhotosException"></exception>
        /// <exception cref="InvalidNumberOfBedroomsException"></exception>
        /// <exception cref="InvalidRoomSizeException"></exception> 
        /// <exception cref="NegativeCurrencyException"></exception>
        /// <exception cref="InvalidCountryException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static Property Create(PropertyDataModel propertyModel, ICollection<BedroomDataModel> bedroomModels, ICollection<PhotoDataModel> photoModels)
        {
            if (propertyModel == null) throw new ArgumentNullException(nameof(PropertyResponse));
            if (bedroomModels == null) throw new ArgumentNullException(nameof(bedroomModels));
            if (photoModels == null) throw new ArgumentNullException(nameof(photoModels));

            if (!Enum.IsDefined(typeof(PropertyTypes), propertyModel.PropertyType))
            {
                throw new InvalidPropertyTypeException(propertyModel.PropertyType.ToString());
            }

            Country country = new Country(propertyModel.Country);

            Bedroom[] bedrooms = bedroomModels.Select(b => new Bedroom(b.Available, b.RoomSize, b.BedSize, b.Rent, b.Deposit)
            {
                Id = b.BedroomId,
                PropertyId = b.PropertyId
            }).ToArray();

            Photo[] photos = photoModels.Select(p => new Photo(p.Uri)
            {
                Id = p.PhotoId,
                PropertyId = p.PropertyId
            }).ToArray();

            Property result = (PropertyTypes)propertyModel.PropertyType switch
            {
                PropertyTypes.Studio => new Studio(bedrooms, photos, country) { Id = propertyModel.Id },
                PropertyTypes.Flat => new Flat(bedrooms, photos, country) { Id = propertyModel.Id },
                PropertyTypes.House => new House(bedrooms, photos, country) { Id = propertyModel.Id },
                _ => throw new InvalidPropertyTypeException(propertyModel.PropertyType.ToString())
            };

            return result;
        }
    }
}
