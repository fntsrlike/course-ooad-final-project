﻿using System;
using System.Windows.Media;

namespace UMLEditort.Entities
{
    /// <summary>
    /// CompositionLine.xaml 的互動邏輯
    /// </summary>
    public partial class GeneralizationLine
    {
        private const int ArrowEndpointHeight = 30;

        public GeneralizationLine(IBaseObject from, IBaseObject to) : base(from, to)
        {
            InitializeComponent();
            var startPoint = From.StartPoint;
            var endPoint = To.StartPoint;
            var xDiff = endPoint.X - startPoint.X;
            var yDiff = endPoint.Y - startPoint.Y;
            var angle = Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI;

            ArrowLine.X1 = ArrowEndpointHeight;
            ArrowLine.X2 = Math.Sqrt(xDiff*xDiff + yDiff*yDiff);
            ArrowCanvas.RenderTransform = new RotateTransform(angle);
        }
    }
}
