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
                NotifyPropertyChanged("ClassBtnBackColor");
                NotifyPropertyChanged("ClassBtnForeColor");
                NotifyPropertyChanged("UseCaseBtnBackColor");
                NotifyPropertyChanged("UseCaseBtnForeColor");
            }
            get { return _mode; }
        }

        public SolidColorBrush ClassBtnBackColor => Mode == Modes.Class ? Brushes.Black : Brushes.White;
        public SolidColorBrush ClassBtnForeColor => Mode == Modes.Class ? Brushes.White : Brushes.Black;
        public SolidColorBrush UseCaseBtnBackColor => Mode == Modes.UseCase ? Brushes.Black : Brushes.White;
        public SolidColorBrush UseCaseBtnForeColor => Mode == Modes.UseCase ? Brushes.White : Brushes.Black;

        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
