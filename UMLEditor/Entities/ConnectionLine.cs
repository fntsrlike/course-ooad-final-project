using System.Windows.Controls;

namespace UMLEditort.Entities
{
    public abstract class ConnectionLine : UserControl
    {
        protected ConnectionLine(IBaseObject from, IBaseObject to)
        {
            From = from;
            To = to;
        }

        public IBaseObject From { get; }
        public IBaseObject To { get; }
    }
}