using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Media;
using UMLEditort.Entities;

namespace UMLEditort
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Modes _mode;

        public ViewModel()
        {
            Mode = Modes.Undefined;
            DiagramCanvas = new DiagramCanvas();
            DiagramCanvas.SelectedRelativeObjects.CollectionChanged += SelectedRelativeObjectsChanged;
            DiagramCanvas.SelectedObjectChanged += delegate {
                NotifyPropertyChanged("CanChangeObjectName");
            };
        }

        /// <summary>
        /// 目前使用者的操作模式
        /// </summary>
        public Modes Mode
        {
            set
            {
                if (_mode == value) return;

                _mode = value;
                DiagramCanvas.Mode = value;
                DiagramCanvas.CleanSelectedObjects();
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

        // 基本屬性
        public DiagramCanvas DiagramCanvas { get; set; }

        // 啟用屬性
        public bool CanChangeObjectName => DiagramCanvas.SelectedObject != null;
        public bool IsGroupEnabled => (DiagramCanvas.SelectedRelativeObjects.Count > 1) && !CheckIfSelectedRelativeObjectsCompositorIsSame();
        public bool IsUnGroupEnabled => (DiagramCanvas.SelectedRelativeObjects.Count > 1) && CheckIfSelectedRelativeObjectsCompositorIsSame();

        // 筆刷屬性
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

        /// <summary>
        /// Group 畫布中的 BaseObjects
        /// </summary>
        public void Group()
        {
            DiagramCanvas.Group();
        }

        /// <summary>
        /// UnGroup 畫布中的 BaseObjects
        /// </summary>
        public void UnGroup()
        {
            DiagramCanvas.UnGroup();
        }

        /// <summary>
        /// 確認選取的 BaseObjects 裡，是否所有 BaseObjects 的最外層 Compositer 都是一樣的
        /// </summary>
        private bool CheckIfSelectedRelativeObjectsCompositorIsSame()
        {
            var isSame = false;

            // 取得作為標準 Compositer
            var compositer = DiagramCanvas.SelectedRelativeObjects[0].GetOutermostCompositer();

            if (compositer == null) return false;
            foreach (var selectedObject in DiagramCanvas.SelectedRelativeObjects)
            {
                isSame = compositer.Equals(selectedObject.GetOutermostCompositer());
                if (!isSame)
                {
                    break;
                }
            }

            return isSame;
        }

        /// <summary>
        /// 選取的 BaseObjects 發生改變
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedRelativeObjectsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("IsGroupEnabled");
            NotifyPropertyChanged("IsUnGroupEnabled");
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
