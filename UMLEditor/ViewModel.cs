using System.ComponentModel;
using System.Windows.Media;

namespace UMLEditort
{
    class ViewModel : INotifyPropertyChanged
    {
        private Modes _mode;

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {
            _mode = Modes.Undefined;
        }

        /// <summary>
        /// 目前使用者的操作模式
        /// </summary>
        public Modes Mode
        {
            set
            {
                _mode = value;
                NotifyPropertyChanged("SelectBtnBackColor");
                NotifyPropertyChanged("SelectBtnForeColor");
                NotifyPropertyChanged("AssociateBtnBackColor");
                NotifyPropertyChanged("AssociateBtnForeColor");
                NotifyPropertyChanged("GeneralizeBtnBackColor");
                NotifyPropertyChanged("GeneralizeBtnForeColor");
                NotifyPropertyChanged("CompositionBtnBackColor");
                NotifyPropertyChanged("CompositionBtnForeColor");
                NotifyPropertyChanged("ClassBtnBackColor");
                NotifyPropertyChanged("ClassBtnForeColor");
                NotifyPropertyChanged("UseCaseBtnBackColor");
                NotifyPropertyChanged("UseCaseBtnForeColor");
            }
            get { return _mode; }
        }

        // 筆刷
        public SolidColorBrush SelectBtnBackColor => Mode == Modes.Select ? Brushes.Black : Brushes.White;
        public SolidColorBrush SelectBtnForeColor => Mode == Modes.Select ? Brushes.White : Brushes.Black;
        public SolidColorBrush AssociateBtnBackColor => Mode == Modes.Associate ? Brushes.Black : Brushes.White;
        public SolidColorBrush AssociateBtnForeColor => Mode == Modes.Associate ? Brushes.White : Brushes.Black;
        public SolidColorBrush GeneralizeBtnBackColor => Mode == Modes.Generalize ? Brushes.Black : Brushes.White;
        public SolidColorBrush GeneralizeBtnForeColor => Mode == Modes.Generalize ? Brushes.White : Brushes.Black;
        public SolidColorBrush CompositionBtnBackColor => Mode == Modes.Composition ? Brushes.Black : Brushes.White;
        public SolidColorBrush CompositionBtnForeColor => Mode == Modes.Composition ? Brushes.White : Brushes.Black;
        public SolidColorBrush ClassBtnBackColor => Mode == Modes.Class ? Brushes.Black : Brushes.White;
        public SolidColorBrush ClassBtnForeColor => Mode == Modes.Class ? Brushes.White : Brushes.Black;
        public SolidColorBrush UseCaseBtnBackColor => Mode == Modes.UseCase ? Brushes.Black : Brushes.White;
        public SolidColorBrush UseCaseBtnForeColor => Mode == Modes.UseCase ? Brushes.White : Brushes.Black;

        public bool GroupMeniItemIsEnalbed
        {
            get { return true; }
        }

        /// <summary>
        /// 用來做 Binding 
        /// </summary>
        /// <param name="propName"></param>
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
