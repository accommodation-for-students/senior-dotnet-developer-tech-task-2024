using AFS.TechTask.Common;
using AFS.TechTask.Properties;

namespace AFS.TechTask.UnitTests.Properties
{
    internal static class PropertyTestConstants
    {
        internal const string LargeRoom = "large";
        internal const string DoubleBed = "double";

        internal static Country UK => new Country(Country.UK);

        internal static IReadOnlyCollection<Bedroom> LargeDoubleBedrooms(int numOfRooms) => Enumerable.Range(0, numOfRooms)
            .Select(_ => new Bedroom(available: true, roomSize: LargeRoom, bedSize: DoubleBed, rent: 350, deposit: 800)).ToArray();

        internal static IReadOnlyCollection<Photo> HousePhotos(int numOfPhotos) => Enumerable.Range(0, numOfPhotos)
            .Select(i => new Photo(uri: $"https://aws.com/images/{i}")).ToArray();
    }
}
