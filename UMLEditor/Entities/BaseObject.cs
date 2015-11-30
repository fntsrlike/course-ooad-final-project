using System.Windows;
using System.Windows.Controls;

namespace UMLEditort.Entities
{
    public abstract class BaseObject : UserControl, IBaseObject
    {
        protected int DiagramWidth = 150;
        protected int DiagramHeight = 100;
        protected int DiagramMargin = 5;

        public abstract bool Selected { get; set; }
        public abstract string ObjectName { get; set; }
        public abstract Point StartPoint { get; set; }
        public abstract Rect GetRect();
        
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public Point EndPoint { get; protected set; }
        public Point TopPoint => new Point(StartPoint.X + (double)DiagramWidth / 2, StartPoint.Y + (double)DiagramMargin / 2);
        public Point RightPoint => new Point(StartPoint.X + DiagramWidth - ((double) DiagramMargin / 2), StartPoint.Y + (double)DiagramHeight / 2);
        public Point BottomPoint => new Point(StartPoint.X + (double)DiagramWidth / 2, StartPoint.Y + DiagramHeight - ((double)DiagramMargin / 2));
        public Point LeftPoint => new Point(StartPoint.X + (double)DiagramMargin / 2, StartPoint.Y + (double)DiagramHeight / 2);

        public CompositeObject Compositer { get; set; }

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

        public bool IsContainPoint(Point point)
        {
            var rect = new Rect(StartPoint, EndPoint);
            return rect.Contains(point);
        }
    }
}
