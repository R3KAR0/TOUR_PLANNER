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
    /// Interaction logic for CreateNewTourWindow.xaml
    /// </summary>
    public partial class CreateNewTourWindow : Window
    {
        public CreateNewTourWindow()
        {
            this.DataContext = new CreateViewModel();
            InitializeComponent();
        }
    }
}
