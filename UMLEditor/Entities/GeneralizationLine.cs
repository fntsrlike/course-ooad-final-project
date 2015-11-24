﻿using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UMLEditort.Entities
{
    class GeneralizationLine : ConnetionLine
    {
        private Line _line;

        public GeneralizationLine(IBaseObject from, IBaseObject to) : base(from, to)
        {
            _line = new Line()
            {
                StrokeThickness = 2,
                Stroke = Brushes.Yellow,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                X1 = From.StartPoint.X,
                Y1 = From.StartPoint.Y,
                X2 = To.StartPoint.X,
                Y2 = To.StartPoint.Y
            };
        }
        
        public override Line GetLine()
        {
            return _line;
        }
    }
}