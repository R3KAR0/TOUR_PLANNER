using LAUER_SWEN2_TOUR_PLANNER.GUI.Exceptions;
using LAUER_SWEN2_TOUR_PLANNER.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LAUER_SWEN2_TOUR_PLANNER.GUI.ViewModels
{
    public class TourLogViewModel : AViewModel
    {
        public TourLog TourLog { get; set; }

        public TourLogViewModel(TourLog tourLog)
        {
            if (tourLog != null)
            {
                TourLog = tourLog;
            }
            else
            {
                throw new ViewModelNullException();
            }
        }
        
        public string Comment
        {
            get => TourLog.Comment.Trim();
        }

        public Difficulty TourDifficulty
        {
            get => TourLog.Difficulty;

        }

        public string TotalTime
        {
            get
            {
                var hours = TourLog.TotalTime / 3600;
                var minutes = TourLog.TotalTime % 60;
                if (hours > 0)
                {
                    return $"{hours} hours(n), {minutes} Minute(n)";
                }
                return $"{minutes} Minute(n)";
            }
            set
            {
                try
                {
                    Int32 seconds;
                    var success = Int32.TryParse(value, out seconds);
                    if(!success)
                    {
                        MessageBox.Show("Please enter the total time in SECONDS![1min = 60 sec, 1hr = 3600 sec]");
                        return;
                    }
                    if(seconds != TourLog.TotalTime)
                    {
                        TourLog.TotalTime = seconds;
                        OnPropertyChanged();
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("Please enter the total time in SECONDS![1min = 60 sec, 1hr = 3600 sec]");
                }


            }
        }

        public Rating TourRating
        {
            get => TourLog.TourRating;
        }

        public DateTime CreationDate
        {
            get => TourLog.CreationDate;
        }
    }
}
