using AFS.TechTask.Application.Properties;
using AFS.TechTask.Application.Properties.Ingest;
using AFS.TechTask.Data.Properties;
using AFS.TechTask.Domain.Common;
using AFS.TechTask.Domain.Properties;

namespace AFS.TechTask.UnitTests.Constants
{
    internal static class PropertyTestConstants
    {
        internal const int PropertyId = 102030;

        internal const string LargeRoom = "large";
        internal const string DoubleBed = "double";
        internal static Country UK => new Country(Country.UK);
        internal static Country ROI => new Country(Country.ROI);

        internal static IReadOnlyCollection<Bedroom> LargeDoubleBedrooms(int numOfRooms) => Enumerable.Range(0, numOfRooms)
            .Select(_ => new Bedroom(available: true, roomSize: LargeRoom, bedSize: DoubleBed, rent: 350, deposit: 800)).ToArray();
        internal static BedroomResponse CreateBedroomResponse(string roomSize = null, string bedSize = null)
            => new BedroomResponse(Available: true, RoomSize: roomSize ?? LargeRoom, BedSize: bedSize ?? DoubleBed, Rent: 350, Deposit: 800);
        internal static ICollection<BedroomResponse> CreateBedroomResponses(int numOfRooms) => Enumerable.Range(0, numOfRooms)
            .Select(_ => CreateBedroomResponse()).ToArray();
        internal static ICollection<BedroomDataModel> BedroomDataModels(int numOfRooms) => Enumerable.Range(0, numOfRooms)
            .Select(i => new BedroomDataModel() { BedroomId = i, PropertyId = PropertyId, Available = true, RoomSize = LargeRoom, BedSize = DoubleBed, Rent = 500, Deposit = 1500 }).ToArray();

        internal static string PhotoUri(int identifier) => $"https://aws.com/images/{identifier}";
        internal static ICollection<string> CreatePhotoResponses(int numOfPhotos) => Enumerable.Range(0, numOfPhotos).Select(PhotoUri).ToArray();
        internal static IReadOnlyCollection<Photo> HousePhotos(int numOfPhotos) => Enumerable.Range(0, numOfPhotos)
            .Select(i => new Photo(uri: PhotoUri(i))).ToArray();
        internal static ICollection<PhotoDataModel> PhotoDataModels(int numOfPhotos) => Enumerable.Range(0, numOfPhotos)
            .Select(i => new PhotoDataModel() { PhotoId = i, Uri = PhotoUri(i), PropertyId = PropertyId }).ToArray();

        internal static PropertyResponse CreatePropertyResponse(string propertyType = null, ICollection<BedroomResponse> bedrooms = null, ICollection<string> photos = null, string country = null)
            => new PropertyResponse(
                PropertyType: propertyType ?? PropertyTypes.House.ToString(),
                Bedrooms: bedrooms ?? [ CreateBedroomResponse(), CreateBedroomResponse(), CreateBedroomResponse() ],
                Photos: photos ?? CreatePhotoResponses(Property.MaxNumberOfPhotos),
                Country: country ?? UK.Name);
        internal static Property CreateProperty(string propertyType = null, ICollection<BedroomResponse> bedrooms = null, ICollection<string> photos = null, string country = null)
            => PropertyFactory.Create(CreatePropertyResponse(
                propertyType: propertyType ?? PropertyTypes.House.ToString(),
                bedrooms: bedrooms ?? [CreateBedroomResponse(), CreateBedroomResponse(), CreateBedroomResponse()],
                photos: photos ?? CreatePhotoResponses(Property.MaxNumberOfPhotos),
                country: country ?? UK.Name));
        internal static PropertyDataModel CreatePropertyDataModel(int? propertyId = null, int? propertyType = null, string country = null)
            => new PropertyDataModel() {
                Id = propertyId ?? PropertyId,
                PropertyType = propertyType ?? (int)PropertyTypes.House,
                Country = country ?? UK.Name,
                IngestRunId = DateTime.MinValue
            };
    }
}
