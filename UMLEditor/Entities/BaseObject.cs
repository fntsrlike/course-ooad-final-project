using System.Windows;
using System.Windows.Controls;

namespace UMLEditort.Entities
{
    public abstract class BaseObject : UserControl, ISelectableObject
    {
        protected int DiagramWidth = 150;
        protected int DiagramHeight = 100;
        protected int DiagramMargin = 5;

        /// <summary>
        /// 物件名稱
        /// </summary>
        public abstract string ObjectName
        {
            get;
            set;
        }

        /// <summary>
        /// 物件起始點位置，等於最左上角的點
        /// </summary>
        public abstract Point StartPoint
        {
            get;
            set;
        }
        
        /// <summary>
        /// 物件終點點位置，等於最左上角的點
        /// </summary>
        public Point EndPoint { get; protected set; }

        /// <summary>
        /// 物件上方連接阜的點位置
        /// </summary>
        public Point TopPoint => new Point(StartPoint.X + (double)DiagramWidth / 2, StartPoint.Y + (double)DiagramMargin / 2);

        /// <summary>
        /// 物件右方連接阜的點位置
        /// </summary>
        public Point RightPoint => new Point(StartPoint.X + DiagramWidth - ((double) DiagramMargin / 2), StartPoint.Y + (double)DiagramHeight / 2);

        /// <summary>
        /// 物件下方連接阜的點位置
        /// </summary>
        public Point BottomPoint => new Point(StartPoint.X + (double)DiagramWidth / 2, StartPoint.Y + DiagramHeight - ((double)DiagramMargin / 2));

        /// <summary>
        /// 物件左方連接阜的點位置
        /// </summary>
        public Point LeftPoint => new Point(StartPoint.X + (double)DiagramMargin / 2, StartPoint.Y + (double)DiagramHeight / 2);


        /// <summary>
        /// 取得本物件涵蓋的矩陣範圍
        /// </summary>
        /// <returns></returns>
        public abstract Rect GetRect();

        /// <summary>
        /// 本物件是否包含某個點
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsContainPoint(Point point)
        {
            var rect = new Rect(StartPoint, EndPoint);
            return rect.Contains(point);
        }

        /// <summary>
        /// 是否已被選取
        /// </summary>
        public abstract bool Selected { get; set; }

        /// <summary>
        /// 本物件所屬的組合物件
        /// </summary>
        public CompositeObject Compositer { get; set; }

        /// <summary>
        /// 本物件所屬組合中的最外層的組合物件
        /// </summary>
        /// <returns></returns>
        public CompositeObject GetOutermostCompositer()
        {
            var compositer = Compositer;

            if (compositer == null)
            {
                return null;
            }

            while (compositer.Compositer != null)
            {
                compositer = compositer.Compositer;
            }
            return compositer;
        }
    }
}
