using AFS.TechTask.Application.Properties.Ingest;
using AFS.TechTask.Data.Properties;
using AFS.TechTask.Domain.Common;
using AFS.TechTask.Domain.Properties;

namespace AFS.TechTask.IntegrationTests.Data.Properties
{
    internal static class PropertyTestConstants
    {
        internal const string LargeRoom = "large";
        internal const string DoubleBed = "double";
        internal static Country UK => new Country(Country.UK);
        internal static Country ROI => new Country(Country.ROI);

        internal static BedroomResponse CreateBedroomResponse(string roomSize = null, string bedSize = null)
            => new BedroomResponse(Available: true, RoomSize: roomSize ?? LargeRoom, BedSize: bedSize ?? DoubleBed, Rent: 350, Deposit: 800);
        internal static ICollection<BedroomDataModel> BedroomDataModels(int propertyId, int numOfRooms) => Enumerable.Range(0, numOfRooms)
            .Select(i => new BedroomDataModel() { BedroomId = i, PropertyId = propertyId, Available = true, RoomSize = LargeRoom, BedSize = DoubleBed, Rent = 500, Deposit = 1500 }).ToArray();

        internal static string PhotoUri(int identifier) => $"https://aws.com/images/{identifier}";
        internal static ICollection<string> CreatePhotoResponses(int numOfPhotos) => Enumerable.Range(0, numOfPhotos).Select(PhotoUri).ToArray();
        internal static ICollection<PhotoDataModel> PhotoDataModels(int propertyId, int numOfPhotos) => Enumerable.Range(0, numOfPhotos)
            .Select(i => new PhotoDataModel() { PhotoId = i, Uri = PhotoUri(i), PropertyId = propertyId }).ToArray();

        internal static PropertyDataModel CreatePropertyDataModel(int? propertyId = null, int? propertyType = null, string country = null)
            => new PropertyDataModel() {
                Id = propertyId ?? 0,
                PropertyType = propertyType ?? (int)PropertyTypes.House,
                Country = country ?? UK.Name,
                IngestRunId = DateTime.MinValue
            };
    }
}
