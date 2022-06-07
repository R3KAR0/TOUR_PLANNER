using LAUER_SWEN2_TOUR_PLANNER.BL.TourLogs;
using LAUER_SWEN2_TOUR_PLANNER.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LAUER_SWEN2_TOUR_PLANNER.GUI.ViewModels
{
    internal class TourLogUpdateView : AViewModel
    {
        private string _windowName = "UpdateTourLogW";
        private bool ready = true;
        private TourLog? _log;

        public TourLogUpdateView(TourLog log)
        {
            _log = log;
        }

        public ICommand UpdateCommand
        {
            get { return new RelayCommand(o => Update(), o => ready); }
        }

        public ICommand CloseCommand
        {
            get { return new RelayCommand(o => Close(), o => ready); }
        }

        public void Close()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.Name == _windowName)
                {

                    window.Close();

                }
            }
        }

        public Rating[] PossibleRatings => new Rating[]
{
             Rating.ONE_STAR, Rating.TWO_STAR, Rating.THREE_STAR, Rating.FOUR_STAR, Rating.FIVE_STAR
};
        public Difficulty[] PossibleDifficulties => new Difficulty[]
        {
            Difficulty.EASY, Difficulty.MEDIUM, Difficulty.HARD, Difficulty.VERY_HARD
        };

        public void Update()
        {
            try
            {
                ready = false;
                TourLogsLogic.UpdateTourLog(_log);
                MessageBox.Show("Updated Tour successfully");
                ready = true;
                Close();
            }
            catch (Exception)
            {
                ready = true;
                MessageBox.Show("Failed to update Tour!");
            }

        }



        public string Comment
        {
            get => _log.Comment;
            set
            {
                _log.Comment = value;
                OnPropertyChanged();
            }
        }
        public string TotalTime
        {
            get => _log.TotalTime.ToString();
            set
            {
                Regex numberRegex = new("^[0-9]+$");
                if (numberRegex.IsMatch(value))
                {
                    _log.TotalTime = Int32.Parse(value);
                    OnPropertyChanged();
                }
            }
        }

        public Difficulty Difficulty
        {
            get => _log.Difficulty;
            set
            {
                _log.Difficulty = value;
                OnPropertyChanged();
            }
        }


        public Rating TourRating
        {
            get => _log.TourRating;
            set
            {
                _log.TourRating = value;
                OnPropertyChanged();
            }
        }
    }
}
