using AFS.TechTask.Domain.Common;

namespace AFS.TechTask.Domain.Properties
{
    /// <summary>
    /// Represents a bedroom in a property for rent.
    /// </summary>
    public class Bedroom
    {
        private static readonly string[] ValidRoomSizes = ["small", "medium", "large"];
        private static readonly string[] ValidBedSizes = ["single", "double", "king size"];

        /// <summary>
        /// Id of the bedroom, once persisted.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// The Id of the property the bedroom is in, once persisted.
        /// </summary>
        public int PropertyId { get; init; }

        /// <summary>
        /// Whether the bedroom is available to rent.
        /// </summary>
        public bool Available { get; }

        /// <summary>
        /// The size of the room (small, medium or large).
        /// </summary>
        public string RoomSize { get; }

        /// <summary>
        /// The size of the bed (single, double or King Size).
        /// </summary>
        public string BedSize { get; }

        /// <summary>
        /// The price to rent the room.
        /// </summary>
        public uint Rent { get; }

        /// <summary>
        /// The price of the deposit to rent the room.
        /// </summary>
        public uint Deposit { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Bedroom"/> class.
        /// </summary>
        public Bedroom(bool available, string roomSize, string bedSize, int rent, int deposit)
        {
            if (!ValidRoomSizes.Contains(roomSize, StringComparer.OrdinalIgnoreCase))
            {
                throw new InvalidRoomSizeException(roomSize);
            }

            if (!ValidBedSizes.Contains(bedSize, StringComparer.OrdinalIgnoreCase)) 
            {
                throw new InvalidBedSizeException(bedSize);
            }

            if (rent < 0)
            {
                throw new NegativeCurrencyException(nameof(Rent), rent);
            }

            if (deposit < 0)
            {
                throw new NegativeCurrencyException(nameof(Deposit), deposit);
            }

            this.Available = available;
            this.RoomSize = roomSize.ToLower();
            this.BedSize = bedSize.ToLower();
            this.Rent = (uint)rent;
            this.Deposit = (uint)deposit;
        }
    }
}
