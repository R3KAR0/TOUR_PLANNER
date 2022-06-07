using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAUER_SWEN2_TOUR_PLANNER.MODEL
{

    public enum Difficulty { EASY, MEDIUM, HARD, VERY_HARD }
    public enum Rating { ONE_STAR, TWO_STAR, THREE_STAR, FOUR_STAR, FIVE_STAR }

    public class TourLog
    {
        public TourLog() { }
        public TourLog(Guid tourId, DateTime creationDate, string comment, Difficulty difficulty, int totalTime, Rating tourRating)
        {
            Id = Guid.NewGuid();
            TourId = tourId;
            CreationDate = creationDate;
            Comment = comment ?? throw new ArgumentNullException(nameof(comment));
            Difficulty = difficulty;
            TotalTime = totalTime;
            TourRating = tourRating;
        }

        public TourLog(Guid id, Guid tourId, DateTime creationDate, string comment, Difficulty difficulty, int totalTime, Rating tourRating)
        {
            Id = id;
            TourId = tourId;
            CreationDate = creationDate;
            Comment = comment ?? throw new ArgumentNullException(nameof(comment));
            Difficulty = difficulty;
            TotalTime = totalTime;
            TourRating = tourRating;
        }

        public Guid Id { get; set; }
        public Guid TourId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Comment { get; set; }
        public Difficulty Difficulty { get; set; }
        public int TotalTime { get; set; }
        public Rating TourRating { get; set; }


    }
}
