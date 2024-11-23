using AFS.TechTask.Common;
using AFS.TechTask.Properties;
using AFS.TechTask.Properties.Ingest.Models;
using AFS.TechTask.Properties.Properties;

namespace AFS.TechTask.UnitTests.Properties
{
    internal static class PropertyTestConstants
    {
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

        internal static string PhotoUri(int identifier) => $"https://aws.com/images/{identifier}";
        internal static ICollection<string> CreatePhotoResponses(int numOfPhotos) => Enumerable.Range(0, numOfPhotos).Select(PhotoUri).ToArray();
        internal static IReadOnlyCollection<Photo> HousePhotos(int numOfPhotos) => Enumerable.Range(0, numOfPhotos)
            .Select(i => new Photo(uri: PhotoUri(i))).ToArray();

        internal static PropertyResponse CreatePropertyResponse(string propertyType = null, ICollection<BedroomResponse> bedrooms = null, ICollection<string> photos = null, string country = null)
            => new PropertyResponse(
                PropertyType: propertyType ?? PropertyTypes.House.ToString(),
                Bedrooms: bedrooms ?? [ CreateBedroomResponse(), CreateBedroomResponse(), CreateBedroomResponse() ],
                Photos: photos ?? CreatePhotoResponses(Property.MaxNumberOfPhotos),
                Country: country ?? UK.Name);
    }
}
