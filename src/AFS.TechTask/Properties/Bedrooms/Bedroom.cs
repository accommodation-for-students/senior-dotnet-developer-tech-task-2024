namespace AFS.TechTask.Properties
{
    /// <summary>
    /// Represents a bedroom in a property for rent.
    /// </summary>
    public class Bedroom
    {
        /// <summary>
        /// Whether the bedroom is available to rent.
        /// </summary>
        public bool Available { get; init; }

        /// <summary>
        /// The size of the room (small, medium or large).
        /// </summary>
        public string RoomSize { get; init; }

        /// <summary>
        /// The size of the bed (single, double or King Size).
        /// </summary>
        public string BedSize { get; init; }

        /// <summary>
        /// The price to rent the room.
        /// </summary>
        public int Rent { get; init; }

        /// <summary>
        /// The price of the deposit to rent the room.
        /// </summary>
        public int Deposit { get; init; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Bedroom"/> class.
        /// </summary>
        public Bedroom(bool available, string roomSize, string bedSize, int rent, int deposit)
        {
            this.Available = available;
            this.RoomSize = roomSize;
            this.BedSize = bedSize;
            this.Rent = rent;
            this.Deposit = deposit;
        }
    }
}
