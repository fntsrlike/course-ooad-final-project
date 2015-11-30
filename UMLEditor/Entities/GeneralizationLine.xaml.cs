using System;
using System.Windows;
using System.Windows.Media;
using UMLEditort.Args;

namespace UMLEditort.Entities
{
    /// <summary>
    /// CompositionLine.xaml 的互動邏輯
    /// </summary>
    public partial class GeneralizationLine
    {
        public GeneralizationLine(ConnectionArgs from, ConnectionArgs to) : base(from, to)
        {
            InitializeComponent();
            ArrowEndpointHeight = 30;

            ArrowLine.X1 = ArrowEndpointHeight;
            ArrowLine.X2 = LineLenght;
            ArrowCanvas.RenderTransform = new RotateTransform(Angle);
        }
    }
}
