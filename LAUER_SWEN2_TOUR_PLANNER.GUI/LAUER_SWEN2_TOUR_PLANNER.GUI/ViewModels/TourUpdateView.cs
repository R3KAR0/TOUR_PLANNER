using LAUER_SWEN2_TOUR_PLANNER.BL.Tours;
using LAUER_SWEN2_TOUR_PLANNER.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LAUER_SWEN2_TOUR_PLANNER.GUI.ViewModels
{
    public class TourUpdateView : AViewModel
    {
        private string _windowName = "UpdateTourW";
        private bool ready = true;
        private Tour? _tour;

        public TourUpdateView(Tour tour)
        {
            _tour = tour;
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

        public async void Update()
        {
            try
            {
                ready = false;
                await TourLogic.UpdateTour(_tour);
                ready = true;
                Close();
            }
            catch (Exception)
            {
                ready = true;
                MessageBox.Show("Failed to update Tour!");
            }

        }



        public string Description
        {
            get => _tour.Description;
            set
            {
                _tour.Description = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get => _tour.Name;
            set
            {
                _tour.Name = value;
                OnPropertyChanged();
            }
        }

        public string From
        {
            get => _tour.From;
            set
            {
                _tour.From = value;
                OnPropertyChanged();
            }
        }


        public string To
        {
            get => _tour.To;
            set
            {
                _tour.To = value;
                OnPropertyChanged();
            }
        }

        public ETransportType TransportType
        {
            get => _tour.TransportType;
            set
            {
                _tour.TransportType = value;
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

    }
}
