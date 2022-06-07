using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LAUER_SWEN2_TOUR_PLANNER.GUI.ViewModels
{
    public class AViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
