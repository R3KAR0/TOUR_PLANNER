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
    /// Interaction logic for UpdateTourLog.xaml
    /// </summary>
    public partial class UpdateTourLogWindow : Window
    {
        public UpdateTourLogWindow(TourLog toUpdate)
        {
            DataContext = new TourLogUpdateView(toUpdate);
            InitializeComponent();
        }
    }
}
