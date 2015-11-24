using System.Windows;

namespace UMLEditort.Entities
{
    /// <summary>
    /// UseCaseObject.xaml 的互動邏輯
    /// </summary>
    public partial class UseCaseObject : IBaseObject
    {
        private Point _startPoint;

        public UseCaseObject()
        {
            InitializeComponent();
        }

        public UseCaseObject(string objectName)
        {
            InitializeComponent();
            ObjectName = objectName;
        }

        public string ObjectName
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

        public Point StartPoint
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

        public Point EndPoint { get; private set; }

        public bool IsContainPoint(Point point)
        {
            var rect = new Rect(StartPoint, EndPoint);
            return rect.Contains(point);
        }
    }
}
