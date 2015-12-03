using System.Windows;

namespace UMLEditort.Entities
{
    /// <summary>
    /// ClassObject.xaml 的互動邏輯
    /// </summary>
    public partial class ClassObject
    {
        private Point _startPoint;
        private bool _selected;

        /// <summary>
        /// 建構子
        /// </summary>
        public ClassObject()
        {
            InitializeComponent();
            _selected = false;
        }

        /// <summary>
        /// 以物件名稱初始化的建構子
        /// </summary>
        /// <param name="objectName">物件名稱</param>
        public ClassObject(string objectName)
        {            
            InitializeComponent();
            _selected = false;
            ObjectNameText.Text = objectName;
        }

        /// <summary>
        /// 物件名稱
        /// </summary>
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

        /// <summary>
        /// 物件起始點位置，等於最左上角的點
        /// </summary>
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

        /// <summary>
        /// 是否已被選取
        /// </summary>
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

        /// <summary>
        /// 取得本物件涵蓋的矩陣範圍
        /// </summary>
        /// <returns></returns>
        public override Rect GetRect()
        {
            var rect = new Rect(StartPoint, EndPoint);
            return rect;
        }
    }
}
