namespace AFS.TechTask.Data.Properties
{
    /// <summary>
    /// Datasource for the Photo table
    /// </summary>
    public interface IPhotosDataSource
    {
        /// <summary>
        /// Insert or update a set of <see cref="PhotoDataModel"/>s, replacing any previous photos.
        /// </summary>
        /// <param name="photos">The photos to upsert.</param>
        Task InsertPhotosAsync(ICollection<PhotoDataModel> photos);

        /// <summary>
        /// Retrieve all <see cref="PhotoDataModel"/>s associated with a property.
        /// </summary>
        /// <param name="propertyId">The Id of the property the photos belong to.</param>
        Task<ICollection<PhotoDataModel>> GetPhotosByPropertyIdAsync(int propertyId);
    }
}
