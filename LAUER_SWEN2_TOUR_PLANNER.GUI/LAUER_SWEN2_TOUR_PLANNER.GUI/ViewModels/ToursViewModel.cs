using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using LAUER_SWEN2_TOUR_PLANNER.BL.TourLogs;
using LAUER_SWEN2_TOUR_PLANNER.BL.Tours;
using LAUER_SWEN2_TOUR_PLANNER.DAL;
using LAUER_SWEN2_TOUR_PLANNER.DAL.Repositories;
using LAUER_SWEN2_TOUR_PLANNER.GUI.Windows;
using LAUER_SWEN2_TOUR_PLANNER.MODEL;
using LAUER_SWEN2_TOUR_PLANNER.BL.CustomExceptions;
using Serilog;
using LAUER_SWEN2_TOUR_PLANNER.BL.Reports;

namespace LAUER_SWEN2_TOUR_PLANNER.GUI.ViewModels
{
    public class ToursViewModel : AViewModel
    {
        private string searchBar = "";
        public string SearchBar { 
            get => searchBar;
            set
            {
                searchBar = value;
                OnPropertyChanged();
            }
        } 
        private TourViewModel? _tourViewModel;
        public TourViewModel? TourViewModel
        {
            get => _tourViewModel;
            set
            {
                if (value == _tourViewModel || value == null) return;
                _tourLogs.Clear();
                _tourViewModel = value;
                _tourViewModel.Tour.Logs.ForEach(tl => _tourLogs.Add(new TourLogViewModel(tl)));
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TourViewModel> Tours { get; set; } = new ObservableCollection<TourViewModel>();

        public ToursViewModel()
        {
            Update();
        }

        public ICommand UpdateTourCommand
        {
            get { return new RelayCommand(o => UpdateTour(), o => TourViewModel != null); }
        }

        private void UpdateTour()
        {
            try
            {
                var updateTourWindow = new EditTourWindow(TourViewModel.Tour);
                updateTourWindow.ShowDialog();
                MessageBox.Show("Update successful!");
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to update tour!");
            }
            Update();
        }


        public ICommand UpdateTourLogCommand
        {
            get { return new RelayCommand(o => UpdateTourLog(), o => _tourLogViewModel != null); }
        }

        private void UpdateTourLog()
        {
            try
            {
                var updateTourLogWindow = new UpdateTourLogWindow(_tourLogViewModel.TourLog);
                updateTourLogWindow.ShowDialog();
                _tourLogs.Clear();
                Update();
                foreach(var tl in TourViewModel.TourLogs)
                {
                    _tourLogs.Add(new(tl));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to update tour!");
            }

        }


        public ICommand CreateSummeryCommand
        {
            get { return new RelayCommand(o => CreateSummery()); }
        }

        public void CreateSummery()
        {
            try
            {
                ReportCreator rc = new();
                rc.CreateSummaryReport();
                MessageBox.Show("Created summery successfully!");
            }
            catch (Exception)
            {

                MessageBox.Show("Something went wrong while exporting the summery :(");
            }

        }

        public ICommand CreateReportOfTour
        {
            get { return new RelayCommand(o => TourReport(), o => TourViewModel != null); }
        }

        public void TourReport()
        {

            try
            {
                ReportCreator rc = new();
                rc.GenerateReportForOneTour(TourViewModel.Tour);
                MessageBox.Show("Created report successfully!");
            }
            catch (Exception)
            {

                MessageBox.Show("Something went wrong while exporting the report :(");
            }
           
        }


        public ICommand DeleteTourCommand
        {
            get { return new RelayCommand(o => DeleteTour(), o => TourViewModel != null); }
        }

        private void DeleteTour()
        {
            try
            {
                TourLogic.DeleteTour(TourViewModel.Tour);
                Tours.Remove(TourViewModel);
                _tourLogs.Clear();
            }
            catch(Exception)
            {
                MessageBox.Show("Failed to delete tour!");
            }

        }

        public ICommand OpenCreateTourViewCommand
        {
            get { return new RelayCommand(o => ShowCreateTourView()); }
        }

        
        private void ShowCreateTourView()
        {
            var createTourView = new CreateNewTourWindow();
            createTourView.ShowDialog();
            Update();
        }

        public ICommand OpenCreateTourLogsViewCommand
        {
            get { return new RelayCommand(o => ShowCreateTourLogsView(), o=> _tourViewModel != null); }
        }
        private void ShowCreateTourLogsView()
        {
            var createTourLogWindow = new CreateTourLogWindow(_tourViewModel.Tour.Id);
            createTourLogWindow.ShowDialog();
            Update();
        }

        public ICommand FullTextSearch
        {
            get { return new RelayCommand(o => FullSearch()); }
        }

        public ICommand ResetCommand
        {
            get { return new RelayCommand(o => Reset()); }
        }

        private void Reset()
        {
            SearchBar = "";
            Update();
        }

        private void FullSearch()
        {
            //not in BL because it is a local search in the ViewModels!
            List<TourViewModel> res = new();

            if(SearchBar == "")
            {
                Update();
                return;
            }

            foreach (var tourModel in Tours)
            {
                if (
                    tourModel.Tour.Name.ToLower().Contains(SearchBar.ToLower()) ||
                    tourModel.Tour.Distance.ToString().Contains(SearchBar.ToLower()) ||
                    tourModel.Tour.From.ToString().ToLower().Contains(SearchBar.ToLower()) ||
                    tourModel.Tour.To.ToLower().Contains(SearchBar.ToLower()) ||
                    tourModel.Tour.Description.ToLower().Contains(SearchBar.ToLower()) ||
                    tourModel.Tour.TransportType.ToString().ToLower().Contains(SearchBar.ToLower())

                    )
                {
                    res.Add(tourModel);
                }

               foreach(var log in tourModel.Tour.Logs)
                {
                    if (
                    log.Comment.ToLower().Contains(SearchBar.ToLower()) ||
                    log.TotalTime.ToString().Contains(SearchBar.ToLower()) ||
                    log.TourRating.ToString().ToLower().Contains(SearchBar.ToLower()) ||
                    log.Difficulty.ToString().ToLower().Contains(SearchBar.ToLower()) 
                    )
                    {
                        if(!res.Contains(tourModel))
                        {
                            res.Add(tourModel);
                        }
                    }
                }
            }

            Tours.Clear();

            if(res.Count == 0)
            {
                MessageBoxResult result = MessageBox.Show("No matches for the given input found!", "No Results");
            }
            else
            {
                res.ForEach(r => Tours.Add(r));
            }
        }

        private void Update()
        {
            Tours.Clear();
            var tours = TourLogic.GetAllToursWithTourLogs();
            tours.ForEach(t => Tours.Add(new(t)));
        }

        public ICommand ExportToursCommand
        {
            get { return new RelayCommand(o => ExportTours(), o => Tours.Count > 0 && Tours != null); }
        }


        public ICommand ImportToursCommand
        {
            get { return new RelayCommand(o => ImportTours()); }
        }


        private void ImportTours()
        {
            try
            {
                IOLogic.ImportTours();
                MessageBox.Show("Tours have been imported successfully");
                Update();
            }
            catch (NoToursToImportException nte)
            {
                MessageBox.Show("There were no Tours to import!");
            }
            catch(ConfigMapperException cme)
            {
                MessageBox.Show("Failed to retreive config mapper!");
            }
            catch (Exception e)
            {
                MessageBox.Show("Import failed!");
                Console.WriteLine(e);
            }
        }

        private async void ExportTours()
        {
            try
            {
                if(await IOLogic.ExportTours())
                {
                    MessageBox.Show("Exported tours successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to export tours!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to export tours [Exception encountered]!");
            }

        }



        private TourLogViewModel? _tourLogViewModel;
        public TourLogViewModel? TourLogViewModel
        {
            get => _tourLogViewModel;
            set
            {
                if (value == _tourLogViewModel) return;
                _tourLogViewModel = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<TourLogViewModel> _tourLogs = new();
        public ObservableCollection<TourLogViewModel> ToursLogs
        {
            get => _tourLogs;
        }

        public ICommand DeleteTourLogCommand
        {
            get { return new RelayCommand(o => DeleteTourLog(), o => TourLogViewModel != null); }
        }

        public void DeleteTourLog()
        {
            try
            {
                TourLogsLogic.DeleteTourLog(TourLogViewModel.TourLog);
                MessageBox.Show("Successfully deleted tourLog!");
                _tourLogs.Remove(TourLogViewModel);
                Update();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to delete tourLog!");
            }
        }

    }
}
