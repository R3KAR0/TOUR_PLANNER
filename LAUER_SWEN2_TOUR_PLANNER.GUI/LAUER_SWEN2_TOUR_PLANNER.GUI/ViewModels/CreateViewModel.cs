using LAUER_SWEN2_TOUR_PLANNER.DAL;
using LAUER_SWEN2_TOUR_PLANNER.DAL.Repositories;
using LAUER_SWEN2_TOUR_PLANNER.MAPQUEST.Requests;
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
    public class CreateViewModel : AViewModel
    {
        private string _windowName = "CreateTourWindow";
        private string _name = "";
        private string _from = "";
        private string _to = "";
        private string _description = "";
        private bool ready = true;

        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }
        public string From
        {
            get => _from;
            set
            {
                if (value == _from) return;
                _from = value;
                OnPropertyChanged();
            }
        }
        public string To
        {
            get => _to;
            set
            {
                if (value == _to) return;
                _to = value;
                OnPropertyChanged();
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                if (value == _description) return;
                _description = value;
                OnPropertyChanged();
            }
        }

        public ETransportType[] PossibleTransportTypes => new ETransportType[] {
        ETransportType.CAR,
        ETransportType.TRAIN,
        ETransportType.SHIP,
        ETransportType.AIRPLANE,
        ETransportType.FOOT,
        ETransportType.BICYCLE,
        ETransportType.MIXED,
        ETransportType.UNKNOWN
        };

        private ETransportType _selectedTransportType;
        public ETransportType SelectedTransportType { 
            get => _selectedTransportType;
            set
            {
                if (value == _selectedTransportType) return;
                _selectedTransportType = value;
                OnPropertyChanged();
            }
        }


        public ICommand CreateTourCommand
        {
            get { return new RelayCommand(o => CreateTour(), o => ready); }
        }

        private async void CreateTour()
        {
            using (var unit = new UnitOfWork())
            {
                ready = false;
                var _repo = unit.TourRepository();
                var duplicationCheck = _repo.GetByName(_name);
                if (duplicationCheck != null)
                {
                    MessageBox.Show("Tour name already taken!");
                    ready = true;
                    return;
                }
                Regex regex = new(@"^[a-zA-Z0-9\x20\-]+$");
                if (Name.Length == 0 || From.Length == 0 || To.Length == 0)
                {
                    MessageBox.Show("You must enter a name, a start and a target location!");
                    ready = true;
                    return;
                }
                if (regex.IsMatch(Name) && regex.IsMatch(From) && regex.IsMatch(To) && (regex.IsMatch(Description) || Description.Length == 0))
                {
                    var res = await RequestRoute.Request(_from, _to, _selectedTransportType);
                    if (res == null)
                    {
                        MessageBox.Show("No route found for given start and target location!");
                        ready = true;
                        return;
                    }
                    var picture = await RequestRoute.GetPicture(res);

                    var result = _repo.Add(new Tour(_name, _description, _from, _to, _selectedTransportType, res.route.distance, res.route.time, DateTime.Now, picture));
                    if (result == null)
                    {
                        MessageBox.Show("Was not able to save tour!");
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
                    MessageBox.Show("Please enter only alphanumeric characters!");
                    ready = true;
                }
            }

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
