using AFS.TechTask.Application.Properties;
using AFS.TechTask.Domain.Properties;
using AFS.TechTask.Infrastructure;
using Microsoft.Extensions.Options;
using Serilog;
using System.Transactions;

namespace AFS.TechTask.Data.Properties
{
    /// <summary>
    /// Repository for <see cref="Property"/> entities.
    /// </summary>
    public class PropertiesRepository : IPropertiesRepository
    {
        private readonly IPropertiesDataSource properties;
        private readonly IBedroomsDataSource bedrooms;
        private readonly IPhotosDataSource photos;
        private readonly FeatureFlagsOptions featureFlags;

        /// <summary>
        /// Initialises a new instance of the <see cref="PropertiesRepository"/> class.
        /// </summary>
        public PropertiesRepository(IPropertiesDataSource properties, IBedroomsDataSource bedrooms, IPhotosDataSource photos,
            IOptions<FeatureFlagsOptions> featureFlags) 
        { 
            this.properties = properties;
            this.bedrooms = bedrooms;
            this.photos = photos;
            this.featureFlags = featureFlags.Value;
        }

        /// <summary>
        /// Insert a <see cref="Property"/>.
        /// </summary>
        /// <param name="ingestRunIdentifier">The timestamp for when the given property was ingested.</param>
        /// <param name="property">The property to add.</param>
        /// <returns>The Id of the created property.</returns>
        public async Task<int> InsertPropertyAsync(DateTime ingestRunIdentifier, Property property)
        {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    int propertyId = await this.properties.InsertPropertyAsync(new PropertyDataModel()
                    {
                        PropertyType = (int)property.Type,
                        Country = property.Country.Name,
                        IngestRunId = ingestRunIdentifier
                    });

                    Task bedroomsTask = this.bedrooms.InsertBedroomsAsync(property.Bedrooms.Select(b => new BedroomDataModel()
                    {
                        PropertyId = property.Id,
                        Available = b.Available,
                        RoomSize = b.RoomSize,
                        BedSize = b.BedSize,
                        Rent = (int)b.Rent,
                        Deposit = (int)b.Deposit
                    }).ToArray());

                    Task photosTask = this.photos.InsertPhotosAsync(property.Photos.Select(b => new PhotoDataModel()
                    {
                        PropertyId = property.Id,
                        Uri = b.Uri
                    }).ToArray());

                    await Task.WhenAll(bedroomsTask, photosTask);

                    transaction.Complete();

                    return propertyId;
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to insert property.");
                throw;
            }
        }

        /// <summary>
        /// Retrieve a <see cref="Property"/> with the given Id.
        /// </summary>
        /// <param name="propertyId">The Id of the property to retrieve.</param>
        /// <returns>The <see cref="Property"/> with the given Id.</returns>
        public async Task<Property> GetPropertyByIdAsync(int propertyId)
        {
            if (!featureFlags.EnablePropertyIngest)
            {
                throw new NotImplementedException();
            }

            try
            {
                Task<PropertyDataModel> propertyTask = this.properties.GetPropertyByIdAsync(propertyId);
                Task<ICollection<BedroomDataModel>> bedroomsTask = this.bedrooms.GetBedroomsByPropertyIdAsync(propertyId);
                Task<ICollection<PhotoDataModel>> photosTask = this.photos.GetPhotosByPropertyIdAsync(propertyId);

                await Task.WhenAll(propertyTask, bedroomsTask, photosTask);

                Property property = PropertyFactory.Create(await propertyTask, await bedroomsTask, await photosTask);
                
                return property;
            }
            catch(Exception e)
            {
                Log.Error(e, "Failed to retrieve property with Id: {PropertyId", propertyId);
                throw;
            }
        }
    }
}
