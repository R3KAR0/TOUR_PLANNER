using LAUER_SWEN2_TOUR_PLANNER.DAL;
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
    public class CreateTourLogView : AViewModel
    {
        private Guid tourId;
        public CreateTourLogView(Guid tourId)
        {
            this.tourId = tourId;
        }

        private string _windowName = "CreateTourLogW";
        private string _duration;
        private string _comment;
        private bool ready = true;

        public string Duration
        {
            get => _duration;
            set
            {
                if (value == _duration) return;
                _duration = value;
                OnPropertyChanged();
            }
        }
        public string Comment
        {
            get => _comment;
            set
            {
                if (value == _comment) return;
                _comment = value;
                OnPropertyChanged();
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

        private Rating _selectedRating;
        public Rating SelectedRating
        {
            get => _selectedRating;
            set
            {
                if (value == _selectedRating) return;
                _selectedRating = value;
                OnPropertyChanged();
            }
        }

        private Difficulty _selectedDifficulty;
        public Difficulty SelectedDifficulty
        {
            get => _selectedDifficulty;
            set
            {
                if (value == _selectedDifficulty) return;
                _selectedDifficulty = value;
                OnPropertyChanged();
            }
        }

        public ICommand CreateTourLogCommand
        {
            get { return new RelayCommand(o => CreateTourLog(), o=>ready); }
        }

        private async void CreateTourLog()
        {
            using (var unit = new UnitOfWork())
            {
                ready = false;
                var _tourRepo = unit.TourRepository();
                var _logsRepo = unit.TourLogRepository();

                if(_tourRepo.GetById(tourId) == null)
                {
                    MessageBox.Show("No corresponding tour found!");
                    ready = true;
                    return;
                }
                
                Regex stringRegex = new(@"[a-zA-Z0-9\x20\-]");
                Regex numberRegex = new("^[0-9]+$");
                if (Duration.Length == 0)
                {
                    MessageBox.Show("You must enter a duration! [in seconds; 60 = 1 min, 3600 = 1hr]");
                    ready = true;
                }
                if (stringRegex.IsMatch(Comment) && numberRegex.IsMatch(Duration))
                {
                    int dur;
                    Int32.TryParse(Duration, out dur);
                    var tourLog = new TourLog(Guid.NewGuid(), tourId, DateTime.Now, Comment, SelectedDifficulty, dur, SelectedRating);
                    var result = _logsRepo.Add(tourLog);
                    if (result == null)
                    {
                        MessageBox.Show("Was not able to save tourLog!");
                        ready = true;
                    }
                    else
                    {
                        ready = true;
                        Close();
                    }

                }
                else
                {
                    MessageBox.Show("Please enter only alphanumeric characters and the duration must be in NUMBERS only!");
                    ready = true;
                }
            }

        }


        public ICommand CloseCommand
        {
            get { return new RelayCommand(o => Close(), o=>ready); }
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

        public void Hide()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.Name == _windowName)
                {

                    window.Hide();
                }
            }
        }

        public void Show()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.Name == _windowName)
                {

                    window.Show();
                }
            }
        }
    }
}
