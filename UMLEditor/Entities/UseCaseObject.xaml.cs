using System.Windows;

namespace UMLEditort.Entities
{
    /// <summary>
    /// UseCaseObject.xaml 的互動邏輯
    /// </summary>
    public partial class UseCaseObject
    {
        private Point _startPoint;
        private bool _selected;

        public UseCaseObject()
        {
            InitializeComponent();
            _selected = false;
        }

        public UseCaseObject(string objectName)
        {
            InitializeComponent();
            _selected = false;
            ObjectNameText.Text = objectName;
        }

        public override string ObjectName
        {
            get
            {
                return ObjectNameText.Text;

            }
            set
            {
                ObjectNameText.Text = value;
            }
        }

        public override Point StartPoint
        {
            get
            {
                return _startPoint;
            }

            set
            {
                _startPoint = value;
                EndPoint = new Point(_startPoint.X + Width, _startPoint.Y + Height);
            }
        }

        public override bool Selected
        {
            get
            {
                return _selected;
            }

            set
            {
                _selected = value;
                var visibility = value ? Visibility.Visible : Visibility.Hidden;
                TopPort.Visibility = visibility;
                RightPort.Visibility = visibility;
                BottomPort.Visibility = visibility;
                LeftPort.Visibility = visibility;
            }
        }

        public override Rect GetRect()
        {
            var rect = new Rect(StartPoint, EndPoint);
            return rect;
        }        
    }
}
