using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using UMLEditort.Entities;

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

        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public BaseObject StartObject { get; set; }
        public BaseObject EndObject { get; set; }
        public ISelectableObject SelectedObject { get; set; }
        public List<ISelectableObject> SelectedRelativeObjects { get; set; }
        public bool PressingFlag { get; set; }
        public bool LineFlag { get; set; }
        public int ObjectCounter { get; set; }

        public bool CanChangeObjectName => SelectedObject != null;

        public void CleanSelectedObjects()
        {
            foreach (var baseObject in SelectedRelativeObjects)
            {
                baseObject.Selected = false;
            }
            SelectedRelativeObjects.Clear();
            SelectedObject = null;
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
