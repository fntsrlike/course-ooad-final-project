using System.Windows;

namespace UMLEditort.Entities
{
    interface IBaseObject
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

        bool Selected
        {
            get;
            set;
        }

        bool IsContainPoint(Point point);
        Rect GetRect();
    }
}
