using System.ComponentModel;
using System.Windows.Media;

namespace UMLEditort
{
    class ViewModel : INotifyPropertyChanged
    {
        private Modes _mode = Modes.Undefined;

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {
            _mode = Modes.Undefined;
        }

        public Modes Mode
        {
            set
            {
                _mode = value;
                NotifyPropertyChanged("ClassBtnColor");
                NotifyPropertyChanged("UseCaseBtnColor");
            }
            get { return _mode; }
        }

        public SolidColorBrush ClassBtnColor => Mode == Modes.Class ? Brushes.Black : Brushes.White;
        public SolidColorBrush UseCaseBtnColor => Mode == Modes.UseCase ? Brushes.Black : Brushes.White;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
