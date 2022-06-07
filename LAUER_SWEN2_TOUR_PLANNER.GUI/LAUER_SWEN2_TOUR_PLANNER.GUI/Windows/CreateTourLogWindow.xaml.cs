using LAUER_SWEN2_TOUR_PLANNER.GUI.ViewModels;
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
    /// Interaction logic for CreateTourLog.xaml
    /// </summary>
    public partial class CreateTourLogWindow : Window
    {
        public CreateTourLogWindow(Guid tourId)
        {
            this.DataContext = new CreateTourLogView(tourId);
            InitializeComponent();
        }
    }
}
