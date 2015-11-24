﻿using System.Windows;

namespace UMLEditort.Entities
{
    interface IBaseObject : ISelectableObject
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
