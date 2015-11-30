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
        
        bool IsContainPoint(Point point);
        Rect GetRect();
    }
}
