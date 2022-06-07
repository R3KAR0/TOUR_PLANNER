namespace LAUER_SWEN2_TOUR_PLANNER.MODEL
{
    public enum ETransportType { CAR, TRAIN, SHIP, AIRPLANE, FOOT, BICYCLE, MIXED, UNKNOWN }
    public class Tour
    {
        public Tour()
        {
            Id = Guid.NewGuid();
        }

        public Tour(string name, string description, string from, string to, ETransportType transportType, double distance, int estimatedTime, DateTime creationDate, byte[] pictureBytes)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            From = from ?? throw new ArgumentNullException(nameof(from));
            To = to ?? throw new ArgumentNullException(nameof(to));
            TransportType = transportType;
            Distance = distance;
            EstimatedTime = estimatedTime;
            CreationDate = creationDate;
            PictureBytes = pictureBytes ?? throw new ArgumentNullException(nameof(pictureBytes));
        }

        public Tour(Guid id, string name, string description, string from, string to, ETransportType transportType, double distance, int estimatedTime, DateTime creationDate, byte[] pictureBytes)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            From = from ?? throw new ArgumentNullException(nameof(from));
            To = to ?? throw new ArgumentNullException(nameof(to));
            TransportType = transportType;
            Distance = distance;
            EstimatedTime = estimatedTime;
            CreationDate = creationDate;
            PictureBytes = pictureBytes ?? throw new ArgumentNullException(nameof(pictureBytes));
        }

        public Tour(Guid id, string name, string description, string from, string to, ETransportType transportType, double distance, int estimatedTime, DateTime creationDate, byte[] pictureBytes, List<TourLog> logs)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description;
            From = from ?? throw new ArgumentNullException(nameof(from));
            To = to ?? throw new ArgumentNullException(nameof(to));
            TransportType = transportType;
            Distance = distance;
            EstimatedTime = estimatedTime;
            CreationDate = creationDate;
            PictureBytes = pictureBytes ?? throw new ArgumentNullException(nameof(pictureBytes));
            Logs = logs ?? throw new ArgumentNullException(nameof(logs));
        }

        public Guid Id { get; set; }    
        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public ETransportType TransportType { get; set; }
        // public float Rating { get; set; } calculated on the fly from DB
        public double Distance { get; set; }
        public int EstimatedTime { get; set; } //DATA TYPE?
        public DateTime CreationDate { get; set; }
        public byte[] PictureBytes { get; set; }

        public List<TourLog> Logs { get; set; } = new();

    }
}
