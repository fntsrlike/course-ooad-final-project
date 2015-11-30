using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UMLEditort.Entities
{
    /// <summary>
    /// GeneralizationLine.xaml 的互動邏輯
    /// </summary>
    public partial class CompositionLine
    {
        private const int ArrowEndpointHeight = 30;

        public CompositionLine(IBaseObject from, IBaseObject to) : base(from, to)
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
