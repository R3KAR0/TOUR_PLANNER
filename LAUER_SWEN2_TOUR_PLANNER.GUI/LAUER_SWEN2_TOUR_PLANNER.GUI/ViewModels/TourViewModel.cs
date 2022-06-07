using LAUER_SWEN2_TOUR_PLANNER.DAL;
using LAUER_SWEN2_TOUR_PLANNER.GUI.Exceptions;
using LAUER_SWEN2_TOUR_PLANNER.MODEL;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace LAUER_SWEN2_TOUR_PLANNER.GUI.ViewModels
{
    public class TourViewModel : AViewModel
    {
        //UnitOfWork unit = new UnitOfWork();
        public Tour Tour { get; private set; }

        public TourViewModel(Tour tour)
        {
            if(tour != null)
            {
                Tour = tour;
            }
            else
            {
                throw new ViewModelNullException();
            }
        }

        public string Description
        {
            get => Tour.Description.Trim();
            set
            {
                Tour.Description = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get => Tour.Name.Trim();
            set
            {
                Tour.Name = value;
                OnPropertyChanged();
            }
        }

        public string From
        {
            get => Tour.From.Trim();
            set
            {
                Tour.From = value;
                OnPropertyChanged();
            }
        }


        public string To
        {
            get => Tour.To.Trim();
            set
            {
                Tour.To = value;
                OnPropertyChanged();
            }
        }
        public double Distance
        {
            get => Tour.Distance;
            set
            {
                return;
                //User.Email = value;
                //OnPropertyChanged();
            }
        }

        public string EstimatedTime
        {
            get
            {
                var hours = Tour.EstimatedTime / 3600;
                var minutes = Tour.EstimatedTime % 60;
                if(hours > 0)
                {
                    return $"{hours} hours(n), {minutes} Minute(n)";
                }
                return $"{minutes} Minute(n)";
            }
            set
            {
                return;
            }
        }
        public DateTime CreationDate
        {
            get => Tour.CreationDate;
            set
            {
                return;
                //User.Email = value;
                //OnPropertyChanged();
            }
        }


        public ObservableCollection<TourLog> TourLogs
        {
            get {
                ObservableCollection<TourLog> logs = new();
                Tour.Logs.ForEach(l => logs.Add(l));
                return logs;
            } 
        }

        public BitmapSource Picture
        {
            get
            {
                try
                {
                    System.Windows.Controls.Image img = new();

                    BitmapImage bitImg = new BitmapImage();

                    bitImg.BeginInit();

                    MemoryStream ms = new MemoryStream(Tour.PictureBytes);

                    bitImg.StreamSource = ms;

                    bitImg.EndInit();

                    img.Source = bitImg;

                    using (MemoryStream outStream = new MemoryStream())
                    {
                        BitmapEncoder enc = new BmpBitmapEncoder();
                        enc.Frames.Add(BitmapFrame.Create(bitImg));
                        enc.Save(outStream);
                        Bitmap bitmap = new Bitmap(outStream);
                        return Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    }   
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed to load map image!");
                    return null;
                }
               
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public string Rating
        {
            get
            {
                if (TourLogs.Count > 0)
                {
                    double sum = 0;
                    foreach (var log in TourLogs)
                    {
                        switch (log.TourRating)
                        {
                            case MODEL.Rating.ONE_STAR:
                                sum += 1;
                                break;
                            case MODEL.Rating.TWO_STAR:
                                sum += 2;
                                break;
                            case MODEL.Rating.THREE_STAR:
                                sum += 3;
                                break;
                            case MODEL.Rating.FOUR_STAR:
                                sum += 4;
                                break;
                            case MODEL.Rating.FIVE_STAR:
                                sum += 5;
                                break;
                        }
                    }
                    return Math.Round((sum / TourLogs.Count), 2).ToString();
                }
                else
                {
                    return "not rated yet!";
                }
            }
        }

        public string MedianDifficulty
        {
            get
            {
                if (TourLogs.Count > 0)
                {
                    var easy = Tour.Logs.FindAll(tl => tl.Difficulty == Difficulty.EASY).Count;
                    var medium = Tour.Logs.FindAll(tl => tl.Difficulty == Difficulty.MEDIUM).Count;
                    var hard = Tour.Logs.FindAll(tl => tl.Difficulty == Difficulty.HARD).Count;
                    var veryhard = Tour.Logs.FindAll(tl => tl.Difficulty == Difficulty.VERY_HARD).Count;

                    return $"Total Votes: \nEASY :{easy} \nMedium: {medium} \nHard: {hard} \nVeryhard {veryhard}\n";

                    
                }
                else
                {
                    return "Difficulty: no feedback yet!";
                }
            }
        }
    }
}