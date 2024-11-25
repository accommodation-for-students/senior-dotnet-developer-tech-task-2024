using System.Data;

namespace AFS.TechTask.Data.Properties
{
    /// <summary>
    /// Datasource for the Photo table
    /// </summary>
    public interface IPhotosDataSource
    {
        /// <summary>
        /// Insert a set of <see cref="PhotoDataModel"/>s for a property.
        /// </summary>
        /// <param name="photos">The photos to insert.</param>
        /// <param name="transaction">Optional transaction to execute within.</param>
        Task InsertPhotosAsync(ICollection<PhotoDataModel> photos, IDbTransaction transaction = null);

        /// <summary>
        /// Replace any existing photos of a property with the given set of <see cref="PhotoDataModel"/>s.
        /// </summary>
        /// <param name="photos">The photos to upsert.</param>
        Task ReplacePhotosAsync(int propertyId, ICollection<PhotoDataModel> photos);

        /// <summary>
        /// Retrieve all <see cref="PhotoDataModel"/>s associated with a property.
        /// </summary>
        /// <param name="propertyId">The Id of the property the photos belong to.</param>
        Task<ICollection<PhotoDataModel>> GetPhotosByPropertyIdAsync(int propertyId);
    }
}
