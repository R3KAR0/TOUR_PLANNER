using LAUER_SWEN2_TOUR_PLANNER.GUI.ViewModels;
using LAUER_SWEN2_TOUR_PLANNER.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LAUER_SWEN2_TOUR_PLANNER.GUI.Windows
{
    /// <summary>
    /// Interaction logic for EditTourWindow.xaml
    /// </summary>
    public partial class EditTourWindow : Window
    {
        public EditTourWindow(Tour tour)
        {
            this.DataContext = new TourUpdateView(tour);
            InitializeComponent();
        }
    }
}
