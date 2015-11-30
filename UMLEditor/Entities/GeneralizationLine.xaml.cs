using System;
using System.Windows;
using System.Windows.Media;

namespace UMLEditort.Entities
{
    /// <summary>
    /// CompositionLine.xaml 的互動邏輯
    /// </summary>
    public partial class GeneralizationLine
    {
        public GeneralizationLine(Point from, Point to) : base(from, to)
        {
            InitializeComponent();
            ArrowEndpointHeight = 30;

            ArrowLine.X1 = ArrowEndpointHeight;
            ArrowLine.X2 = LineLenght;
            ArrowCanvas.RenderTransform = new RotateTransform(Angle);
        }
    }
}
