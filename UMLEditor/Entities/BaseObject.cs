using System.Windows;
using System.Windows.Controls;

namespace UMLEditort.Entities
{
    public abstract class BaseObject : UserControl, IBaseObject
    {
        public abstract bool Selected { get; set; }
        public abstract string ObjectName { get; set; }
        public abstract Point StartPoint { get; set; }        
        public abstract Rect GetRect();
        
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public Point EndPoint { get; protected set; }

        public CompositeObject Compositer { get; set; }

        public bool IsContainPoint(Point point)
        {
            var rect = new Rect(StartPoint, EndPoint);
            return rect.Contains(point);
        }
    }
}
