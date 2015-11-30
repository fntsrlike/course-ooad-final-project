using System;
using System.Windows;
using System.Windows.Media;

namespace UMLEditort.Entities
{
    /// <summary>
    /// GeneralizationLine.xaml 的互動邏輯
    /// </summary>
    public partial class AssociationLine
    {
        public AssociationLine(Point from, Point to) : base(from, to)
        {
            InitializeComponent();
            ArrowEndpointHeight = 0;

            ArrowLine.X1 = ArrowEndpointHeight;
            ArrowLine.X2 = LineLenght;
            ArrowCanvas.RenderTransform = new RotateTransform(Angle);
        }
    }
}
