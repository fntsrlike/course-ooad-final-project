using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UMLEditort.Entities
{
    class Association
    {
        public Association(IBaseObject from, IBaseObject to)
        {
            From = from;
            To = to;

            AssociationLine = new Line()
            {
                StrokeThickness = 2,
                Stroke = Brushes.Black,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                X1 = From.StartPoint.X,
                Y1 = From.StartPoint.Y,
                X2 = To.StartPoint.X,
                Y2 = To.StartPoint.Y
            };
        }

        public IBaseObject From { get; }
        public IBaseObject To { get; }

        public Line AssociationLine { get; }
    }
}
