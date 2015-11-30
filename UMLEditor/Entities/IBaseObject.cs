using System.Windows;

namespace UMLEditort.Entities
{
    public interface IBaseObject : ISelectableObject
    {
        string ObjectName
        {
            get;
            set;
        }

        Point StartPoint
        {
            get;
            set;
        }

        Point TopPoint
        {
            get;
        }

        Point RightPoint
        {
            get;
        }

        Point BottomPoint
        {
            get;
        }

        Point LeftPoint
        {
            get;
        }

        bool IsContainPoint(Point point);
        Rect GetRect();
    }
}
